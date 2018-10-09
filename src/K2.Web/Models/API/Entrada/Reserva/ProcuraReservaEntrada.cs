namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para realizar a procura por reservas
    /// </summary>
    public class ProcurarReservaEntrada : ProcurarEntrada
    {
        public int? IdViagem { get; set; }

        public int? IdCliente { get; set; }

        public decimal? ValorPago { get; set; }

        public string Observacao { get; set; }
    }
}
