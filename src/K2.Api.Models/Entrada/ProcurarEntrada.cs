namespace K2.Api.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros básicos para realizar uma procura
    /// </summary>
    public abstract class ProcurarEntrada : BaseModel
    {
        /// <summary>
        /// Página atual da listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaIndex { get; set; }

        /// <summary>
        /// Quantidade de registros exibidos por página na listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaTamanho { get; set; }

        /// <summary>
        /// Nome da propriedade que deverá ser utilizada para ordenação do resultado da pesquisa
        /// </summary>
        public string OrdenarPor { get; set; }

        /// <summary>
        /// Sentido da ordenação do resultado da pesquisa
        /// </summary>
        public string OrdenarSentido { get; set; }

        public ProcurarEntrada()
        {
            this.OrdenarSentido = "ASC";
        }
    }
}
