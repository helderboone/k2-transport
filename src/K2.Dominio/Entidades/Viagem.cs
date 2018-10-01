using K2.Dominio.Comandos.Entrada;
using System;
using System.Linq;

namespace K2.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma localidade
    /// </summary>
    public class Viagem
    {
        /// <summary>
        /// ID da viagem
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// ID do carro
        /// </summary>
        public int IdCarro { get; private set; }

        /// <summary>
        /// ID do motorista
        /// </summary>
        public int IdMotorista { get; private set; }

        /// <summary>
        /// ID da localidade de embarque
        /// </summary>
        public int IdLocalidadeEmbarque { get; private set; }

        /// <summary>
        /// ID da localidade de desembarque
        /// </summary>
        public int IdLocalidadeDesembarque { get; private set; }

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
        /// Situação da viagem
        /// </summary>
        public int Situacao { get; private set; }

        /// <summary>
        /// Descrição dos locais de embarque
        /// </summary>
        public string Embarques { get; private set; }

        /// <summary>
        /// Descrição dos locais de desembarque
        /// </summary>
        public string Desembarques { get; private set; }

        /// <summary>
        /// Descrição do motivo de cancelamento da viagem
        /// </summary>
        public string DescricaoCancelamento { get; private set; }

        public Carro Carro { get; private set; }

        public Motorista Motorista { get; private set; }

        public Localidade LocalidadeEmbarque { get; private set; }

        public Localidade LocalidadeDesembarque { get; private set; }

        private Viagem()
        {

        }

        public Viagem(CadastrarViagemEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.IdCarro                 = entrada.IdCarro;
            this.IdMotorista             = entrada.IdMotorista;
            this.IdLocalidadeEmbarque    = entrada.IdLocalidadeEmbarque;
            this.IdLocalidadeDesembarque = entrada.IdLocalidadeDesembarque;
            this.Descricao               = entrada.Descricao;
            this.ValorPassagem           = entrada.ValorPassagem;
            this.DataHorarioSaida        = entrada.DataHorarioSaida;
            this.Embarques               = entrada.LocaisEmbarque != null && entrada.LocaisEmbarque.Any()
                ? string.Join(";", entrada.LocaisEmbarque)
                : null;
            this.Desembarques            = entrada.LocaisDesembarque != null && entrada.LocaisDesembarque.Any()
                ? string.Join(";", entrada.LocaisDesembarque)
                : null;
        }

        public void Alterar(AlterarViagemEntrada entrada)
        {
            if (entrada.Invalido || entrada.Id != this.Id)
                return;

            this.Id                      = entrada.Id;
            this.IdCarro                 = entrada.IdCarro;
            this.IdMotorista             = entrada.IdMotorista;
            this.IdLocalidadeEmbarque    = entrada.IdLocalidadeEmbarque;
            this.IdLocalidadeDesembarque = entrada.IdLocalidadeDesembarque;
            this.Descricao               = entrada.Descricao;
            this.ValorPassagem           = entrada.ValorPassagem;
            this.DataHorarioSaida        = entrada.DataHorarioSaida;
            this.Embarques               = entrada.LocaisEmbarque != null && entrada.LocaisEmbarque.Any()
                ? string.Join(";", entrada.LocaisEmbarque)
                : null;
            this.Desembarques            = entrada.LocaisDesembarque != null && entrada.LocaisDesembarque.Any()
                ? string.Join(";", entrada.LocaisDesembarque)
                : null;
        }

        public override string ToString()
        {
            return $"{this.Descricao} - {this.DataHorarioSaida.ToString("dd/MM/yyyy HH:mm")}";
        }
    }
}
