using K2.Dominio.Comandos.Entrada;
using System;

namespace K2.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um dependente de uma reserva
    /// </summary>
    public class ReservaDependente
    {
        /// <summary>
        /// ID da reserva
        /// </summary>
        public int IdReserva { get; private set; }

        /// <summary>
        /// Nome do dependente
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Data de nascimento do dependente
        /// </summary>
        public DateTime DataNascimento { get; private set; }

        /// <summary>
        /// CPF do dependente
        /// </summary>
        public string Cpf { get; private set; }

        /// <summary>
        /// RG do dependente
        /// </summary>
        public string Rg { get; private set; }

        private ReservaDependente()
        {

        }

        public ReservaDependente(CadastrarReservaDependenteEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.IdReserva      = entrada.IdReserva;
            this.Nome           = entrada.Nome;
            this.DataNascimento = entrada.DataNascimento;
            this.Cpf            = entrada.Cpf;
            this.Rg             = entrada.Rg;
        }

        public void Alterar(AlterarReservaDependenteEntrada entrada)
        {
            if (entrada.Invalido || entrada.IdReserva != this.IdReserva)
                return;

            this.Nome = entrada.Nome;
            this.DataNascimento = entrada.DataNascimento;
            this.Cpf = entrada.Cpf;
            this.Rg = entrada.Rg;
        }
    }
}
