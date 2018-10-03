using JNogueira.Infraestrutura.NotifiqueMe;
using JNogueira.Infraestrutura.Utilzao;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Resources;
using System;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de um dependente de uma reserva
    /// </summary>
    public class CadastrarReservaDependenteEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// ID da reserva
        /// </summary>
        public int IdReserva { get; }

        /// <summary>
        /// Nome do dependente
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Data de nascimento do dependente
        /// </summary>
        public DateTime DataNascimento { get; }

        /// <summary>
        /// CPF do dependente
        /// </summary>
        public string Cpf { get; }

        /// <summary>
        /// RG do dependente
        /// </summary>
        public string Rg { get; }

        public CadastrarReservaDependenteEntrada(
            int idReserva,
            string nome,
            DateTime dataNascimento,
            string cpf,
            string rg)
        {
            IdReserva      = idReserva;
            Nome           = nome?.ToUpper();
            DataNascimento = dataNascimento;
            Cpf            = cpf?.RemoverCaracter(".", "-", "/");
            Rg             = rg?.ToUpper().RemoverCaracter(".", "-", "/");

            this.Validar();
        }

        private void Validar()
        {
            var dataHoraBrasilia = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"));

            this
                .NotificarSeMenorOuIgualA(this.IdReserva, 0, ReservaResource.Id_Reserva_Nao_Existe)
                .NotificarSeVerdadeiro(dataHoraBrasilia.Date.Subtract(this.DataNascimento.Date).TotalDays > 6 * 365, ReservaDependenteResource.Idade_Maxima_Superior_6_Anos)
                .NotificarSeNuloOuVazio(this.Nome, ClienteResource.Id_Cliente_Nao_Existe);

            if (!string.IsNullOrEmpty(this.Cpf))
                this.NotificarSeFalso(this.Cpf.ValidarCpf(), UsuarioResource.Cpf_Invalido);
        }
    }
}
