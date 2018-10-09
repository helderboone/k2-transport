using System;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para realizar a procura por viagens
    /// </summary>
    public class ProcurarViagemEntrada : ProcurarEntrada
    {
        public int? IdCarro { get; set; }

        public int? IdMotorista { get; set; }

        public int? IdLocalidadeEmbarque { get; set; }

        public int? IdLocalidadeDesembarque { get; set; }

        public string Descricao { get; set; }

        public decimal? ValorPassagem { get; set; }

        public DateTime? DataSaidaInicio { get; set; }

        public DateTime? DataSaidaFim { get; set; }

        public bool? SomentePrevistas { get; set; }

        public bool? SomenteRealizadasOuCanceladas { get; set; }
    }
}
