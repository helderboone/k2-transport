namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros básicos para realizar uma procura
    /// </summary>
    public abstract class ProcurarEntrada : BaseModel
    {
        public int? PaginaIndex { get; set; }

        public int? PaginaTamanho { get; set; }

        public string OrdenarPor { get; set; }

        public string OrdenarSentido { get; set; }

        public ProcurarEntrada()
        {
            this.OrdenarSentido = "ASC";
        }
    }
}
