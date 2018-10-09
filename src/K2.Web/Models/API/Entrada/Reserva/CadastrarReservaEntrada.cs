namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para o cadastro de uma reserva
    /// </summary>
    public class CadastrarReservaEntrada : BaseModel
    {
        public int IdViagem { get; set; }

        public int IdCliente { get; set; }

        public decimal? ValorPago { get; set; }

        public string Observacao { get; set; }
    }
}
