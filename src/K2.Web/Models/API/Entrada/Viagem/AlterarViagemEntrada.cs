using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Resources;
using System;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o alterar uma viagem
    /// </summary>
    public class AlterarViagemEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// ID da viagem
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// ID do carro
        /// </summary>
        public int IdCarro { get; }

        /// <summary>
        /// ID do motorista
        /// </summary>
        public int IdMotorista { get; }

        /// <summary>
        /// ID da localidade de embarque
        /// </summary>
        public int IdLocalidadeEmbarque { get; }

        /// <summary>
        /// ID da localidade de desembarque
        /// </summary>
        public int IdLocalidadeDesembarque { get; }

        /// <summary>
        /// Descrição da viagem
        /// </summary>
        public string Descricao { get; private set; }

        /// <summary>
        /// Valor da passagem por pessoa
        /// </summary>
        public decimal ValorPassagem { get; private set; }

        /// <summary>
        /// Data e horário da saída
        /// </summary>
        public DateTime DataHorarioSaida { get; private set; }

        /// <summary>
        /// Descrição dos locais de embarque
        /// </summary>
        public string[] LocaisEmbarque { get; private set; }

        /// <summary>
        /// Descrição dos locais de desembarque
        /// </summary>
        public string[] LocaisDesembarque { get; private set; }

        public AlterarViagemEntrada(
            int id,
            int idCarro,
            int idMotorista,
            int idLocalidadeEmbarque,
            int idLocalidadeDesembarque,
            string descricao,
            decimal valorPassagem,
            DateTime dataHorarioSaida,
            string[] locaisEmbarque,
            string[] locaisDesembarque)
        {
            Id                      = id;
            IdCarro                 = idCarro;
            IdMotorista             = idMotorista;
            IdLocalidadeEmbarque    = idLocalidadeEmbarque;
            IdLocalidadeDesembarque = idLocalidadeDesembarque;
            Descricao               = descricao;
            ValorPassagem           = valorPassagem;
            DataHorarioSaida        = dataHorarioSaida;
            LocaisEmbarque          = locaisEmbarque;
            LocaisDesembarque       = locaisDesembarque;

            Validar();
        }

        public override string ToString()
        {
            return $"{this.Descricao} - {this.DataHorarioSaida.ToString("dd/MM/yyyy HH:mm")}";
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.Id, 0, ViagemResource.Id_Viagem_Nao_Existe)
                .NotificarSeMenorOuIgualA(this.IdCarro, 0, CarroResource.Id_Carro_Nao_Existe)
                .NotificarSeMenorOuIgualA(this.IdMotorista, 0, MotoristaResource.Id_Motorista_Nao_Existe)
                .NotificarSeMenorOuIgualA(this.IdLocalidadeEmbarque, 0, ViagemResource.Localidade_Embarque_Nao_Existe)
                .NotificarSeMenorOuIgualA(this.IdLocalidadeDesembarque, 0, ViagemResource.Localidade_Desembarque_Nao_Existe)
                .NotificarSeNuloOuVazio(this.Descricao, ViagemResource.Descricao_Obrigatoria_Nao_Informada)
                .NotificarSeMenorOuIgualA(this.ValorPassagem, 0, ViagemResource.Valor_Passagem_Obrigatorio_Nao_Informada);
        }
    }
}
