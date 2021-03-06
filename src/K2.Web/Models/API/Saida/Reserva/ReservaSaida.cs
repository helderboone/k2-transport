﻿using JNogueira.Infraestrutura.Utilzao;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de uma viagem
    /// </summary>
    public class ReservaSaida : Saida<ReservaRegistro>
    {
        public ReservaSaida(bool sucesso, IEnumerable<string> mensagens, ReservaRegistro retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public static ReservaSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ReservaSaida>(json)
                : throw new Exception("A saida da API foi nula ou vazia.");
        }
    }

    /// <summary>
    /// Classe que reflete as informações de saída de uma viagem
    /// </summary>
    public class ReservasSaida : Saida<IEnumerable<ReservaRegistro>>
    {
        public ReservasSaida(bool sucesso, IEnumerable<string> mensagens, IEnumerable<ReservaRegistro> retorno)
            : base(sucesso, mensagens, retorno)
        {

        }

        public static ReservasSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ReservasSaida>(json)
                : throw new Exception("A saida da API foi nula ou vazia.");
        }
    }

    public enum TipoReservaPagamento
    {
        Pago = 1,
        Parcial = 2,
        Pendente = 0
    }

    public class ReservaRegistro
    {
        public int Id { get; set; }

        public int IdCliente { get; set; }

        public decimal? ValorPago { get; set; }

        public string ValorPagoFormatado => this.ValorPago?.ToString("C2");

        public string LocalEmbarque { get; set; }

        public string LocalDesembarque { get; set; }

        public int SequenciaEmbarque { get; set; }

        public string Observacao { get; set; }

        public ReservaClienteRetorno Cliente { get; set; }

        public ReservaViagemRetorno Viagem { get; set; }

        public ReservaDependenteRetorno Dependente { get; set; }

        public int Pago => this.ValorPago.HasValue && this.ValorPago.Value >= this.Viagem.ValorPassagem
            ? (int)TipoReservaPagamento.Pago
            : (this.ValorPago.HasValue && this.ValorPago.Value != 0 && this.ValorPago.Value != this.Viagem.ValorPassagem
                ? (int)TipoReservaPagamento.Parcial
                : (int)TipoReservaPagamento.Pendente);
    }

    public class ReservaClienteRetorno
    {
        public string Nome { get; set; }

        public string Rg { get; set; }

        public string Cpf { get; set; }

        public string CpfFormatado => this.Cpf.FormatarCpf();

        public string Celular { get; set; }

        public string CelularFormatado => this.Celular.Formatar("(##)######-####");
    }

    public class ReservaViagemRetorno
    {
        public int Id { get; set; }

        public string Descricao { get; set; }

        public DateTime DataHorarioSaida { get; set; }

        public string DescricaoSituacao { get; set; }

        public decimal ValorPassagem { get; set; }

        public ReservaCarroRetorno Carro { get; set; }

        public ReservaMotoristaRetorno Motorista { get; set; }

        public ReservaLocalidadeRetorno LocalidadeEmbarque { get; set; }

        public ReservaLocalidadeRetorno LocalidadeDesembarque { get; set; }

        public string[] LocaisEmbarque { get; set; }

        public string[] LocaisDesembarque { get; set; }
    }

    public class ReservaCarroRetorno
    {
        public string Descricao { get; set; }

        public string Placa { get; set; }

        public int Capacidade { get; set; }
    }

    public class ReservaMotoristaRetorno
    {
        public string Nome { get; set; }

        public string Cnh { get; set; }

        public string Cpf { get; set; }

        public string CpfFormatado => this.Cpf.FormatarCpf();

        public string Celular { get; set; }

        public string CelularFormatado => this.Celular.Formatar("(##)######-####");
    }

    public class ReservaLocalidadeRetorno
    {
        public string Nome  { get; set; }

        public string Sigla { get; set; }

        public string Uf { get; set; }
    }

    public class ReservaDependenteRetorno
    {
        public int IdReserva { get; set; }

        public string Nome { get; set; }

        public DateTime DataNascimento { get; set; }

        public string DataNascimentoToString => this.DataNascimento.ToString("dd/MM/yyyy");

        public string Cpf { get; set; }

        public string CpfFormatado => this.Cpf.FormatarCpf();

        public string Rg { get; set; }
    }
}
