using JNogueira.Infraestrutura.Utilzao;
using K2.Dominio.Comandos.Entrada;
using System;
using System.Collections.Generic;
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
        /// KM inicial
        /// </summary>
        public int? KmInicial { get; private set; }

        /// <summary>
        /// KM final
        /// </summary>
        public int? KmFinal { get; private set; }

        /// <summary>
        /// KM rodado
        /// </summary>
        public int? KmRodado { get; private set; }

        /// <summary>
        /// Nome do contratante do frete
        /// </summary>
        public string NomeContratanteFrete { get; private set; }

        /// <summary>
        /// Endereço do contratante do frete
        /// </summary>
        public string EnderecoContratanteFrete { get; private set; }

        /// <summary>
        /// Número do contratante do frete
        /// </summary>
        public string DocumentoContratanteFrete { get; private set; }

        /// <summary>
        /// RG do contratante do frete
        /// </summary>
        public string RgContratanteFrete { get; private set; }

        /// <summary>
        /// Telefone do contratante do frete
        /// </summary>
        public string TelefoneContratanteFrete { get; private set; }

        /// <summary>
        /// Carro designado para realizar a viagem
        /// </summary>
        public Carro Carro { get; private set; }

        /// <summary>
        /// Motorista designado para realizar a viagem
        /// </summary>
        public Motorista Motorista { get; private set; }

        /// <summary>
        /// Localidade de embarque da viagem
        /// </summary>
        public Localidade LocalidadeEmbarque { get; private set; }

        /// <summary>
        /// Localidade de desembarque da viagem
        /// </summary>
        public Localidade LocalidadeDesembarque { get; private set; }

        /// <summary>
        /// Reservas realizadas para a viagem
        /// </summary>
        public IEnumerable<Reserva> Reservas { get; private set; }

        /// <summary>
        /// Percentual de disponibilidade da viagem
        /// </summary>
        public decimal PercentualDisponibilidade => !this.Reservas.Any() ? 100 : 100 - (this.Reservas.Count() * 100 / this.Carro.Capacidade);

        /// <summary>
        /// Quantidade de lugares disponíveis
        /// </summary>
        public int QuantidadeLugaresDisponiveis => this.Carro.Capacidade - this.Reservas.Count();

        protected Viagem()
        {
            this.Reservas = new List<Reserva>();
        }

        public Viagem(CadastrarViagemEntrada entrada)
            : base()
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
            this.Embarques               = entrada.LocaisEmbarque?.Any() == true
                ? string.Join(";", entrada.LocaisEmbarque)
                : null;
            this.Desembarques            = entrada.LocaisDesembarque?.Any() == true
                ? string.Join(";", entrada.LocaisDesembarque)
                : null;
            this.KmInicial                 = entrada.KmInicial;
            this.KmFinal                   = entrada.KmFinal;
            this.KmRodado                  = entrada.KmRodado;
            this.NomeContratanteFrete      = entrada.NomeContratanteFrete;
            this.EnderecoContratanteFrete  = entrada.EnderecoContratanteFrete;
            this.DocumentoContratanteFrete = entrada.DocumentoContratanteFrete;
            this.RgContratanteFrete        = entrada.RgContratanteFrete;
            this.TelefoneContratanteFrete  = entrada.TelefoneContratanteFrete;

            this.Situacao = (int)TipoSituacaoViagem.PendenteConfirmacao;
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
            this.Embarques               = entrada.LocaisEmbarque?.Any() == true
                ? string.Join(";", entrada.LocaisEmbarque)
                : null;
            this.Desembarques            = entrada.LocaisDesembarque?.Any() == true
                ? string.Join(";", entrada.LocaisDesembarque)
                : null;
            this.KmInicial                 = entrada.KmInicial;
            this.KmFinal                   = entrada.KmFinal;
            this.KmRodado                  = entrada.KmRodado;
            this.NomeContratanteFrete      = entrada.NomeContratanteFrete;
            this.EnderecoContratanteFrete  = entrada.EnderecoContratanteFrete;
            this.DocumentoContratanteFrete = entrada.DocumentoContratanteFrete;
            this.RgContratanteFrete        = entrada.RgContratanteFrete;
            this.TelefoneContratanteFrete  = entrada.TelefoneContratanteFrete;
        }

        public TipoSituacaoViagem ObterTipoSituacao()
        {
            return DateTimeHelper.ObterHorarioAtualBrasilia() > this.DataHorarioSaida
                ? TipoSituacaoViagem.Realizada
                : this.Situacao.ConverterParaEnum(TipoSituacaoViagem.PendenteConfirmacao);
        }

        public override string ToString()
        {
            return $"{this.Descricao} - {this.DataHorarioSaida.ToString("dd/MM/yyyy HH:mm")}";
        }
    }
}
