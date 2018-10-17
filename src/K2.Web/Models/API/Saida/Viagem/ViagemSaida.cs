using K2.Infraestrutura;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de uma viagem
    /// </summary>
    public class ViagemSaida : Saida<ViagemRegistro>
    {
        public ViagemSaida(bool sucesso, IEnumerable<string> mensagens, ViagemRegistro retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public static ViagemSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ViagemSaida>(json)
                : null;
        }
    }

    /// <summary>
    /// Classe que reflete as informações de saída de uma viagem
    /// </summary>
    public class ViagensSaida : Saida<IEnumerable<ViagemRegistro>>
    {
        public ViagensSaida(bool sucesso, IEnumerable<string> mensagens, IEnumerable<ViagemRegistro> retorno)
            : base(sucesso, mensagens, retorno)
        {

        }

        public static ViagensSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ViagensSaida>(json)
                : null;
        }
    }

    public class ViagemRegistro
    {
        public int Id { get; set; }

        public int IdCarro { get; set; }

        public int IdMotorista { get; set; }

        public int IdLocalidadeEmbarque { get; set; }

        public int IdLocalidadeDesembarque { get; set; }

        public string Descricao { get; set; }

        public decimal ValorPassagem { get; set; }

        public DateTime DataHorarioSaida { get; set; }

        public int Situacao { get; set; }

        public string DescricaoSituacao { get; set; }

        public string[] LocaisEmbarque { get; set; }

        public string[] LocaisDesembarque { get; set; }

        public string DescricaoCancelamento { get; set; }

        public ViagemCarroRetorno Carro { get; set; }

        public ViagemMotoristaRetorno Motorista { get; set; }

        public ViagemLocalidadeRetorno LocalidadeEmbarque { get; set; }

        public ViagemLocalidadeRetorno LocalidadeDesembarque { get; set; }

        public IEnumerable<ViagemReservaRetorno> Reservas { get; set; }

        public string PercentualDisponibilidade { get; set; }

        public int QuantidadeLugaresDisponiveis { get; set; }

        public string DataHorarioSaidaToString => this.DataHorarioSaida.ToString("dd MMM, yyyy - HH:mm");

        public double QuantidadeDiasSaida => Math.Round(this.DataHorarioSaida.Subtract(DateTimeHelper.ObterHorarioAtualBrasilia()).TotalDays, 0);

        public decimal ValorArrecadadoReservas => this.Reservas.Where(x => x.ValorPago.HasValue).Sum(x => x.ValorPago.Value);

        public decimal PercentualArrecadadoReservas => Math.Round(100 * this.ValorArrecadadoReservas / (this.Carro.QuantidadeLugares * this.ValorPassagem), 0);
    }

    public class ViagemCarroRetorno
    {
        public string Descricao { get; set; }

        public string NomeProprietario { get; set; }

        public string AnoModelo { get; set; }

        public string NomeFabricante { get; set; }

        public string Renavam { get; set; }

        public string[] Caracteristicas { get; set; }

        public string Placa { get; set; }

        public int QuantidadeLugares { get; set; }
    }

    public class ViagemMotoristaRetorno
    {
        public string Nome { get; set; }

        public string Cnh { get; set; }

        public string Cpf { get; set; }

        public string Celular { get; set; }
    }

    public class ViagemLocalidadeRetorno
    {
        public string Nome  { get; set; }

        public string Uf { get; set; }
    }

    public class ViagemReservaRetorno
    {
        public int Id { get; set; }

        public int IdCliente { get; set; }

        public string NomeCliente { get; set; }

        public int Situacao { get; set; }

        public string DescricaoSituacao { get; set; }

        public string Observacao { get; set; }

        public decimal? ValorPago { get; set; }
    }
}
