namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para alteração de uma reserva
    /// </summary>
    public class AlterarReservaEntrada : BaseModel
    {
        public int Id { get; set; }

        public int IdCliente { get; set; }

        public decimal? ValorPago { get; set; }

        public string Observacao { get; set; }
    }
}
