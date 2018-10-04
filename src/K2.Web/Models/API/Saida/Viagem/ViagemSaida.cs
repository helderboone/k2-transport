using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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

        public object Carro { get; set; }

        public object Motorista { get; set; }

        public object LocalidadeEmbarque { get; set; }

        public object LocalidadeDesembarque { get; set; }

        public string PercentualDisponibilidade { get; set; }

        public int QuantidadeLugaresDisponiveis { get; set; }
    }
}
