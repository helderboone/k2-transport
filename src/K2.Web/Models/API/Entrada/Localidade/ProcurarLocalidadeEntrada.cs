namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para realizar a procura por localidades
    /// </summary>
    public class ProcurarLocalidadeEntrada : ProcurarEntrada
    {
        /// <summary>
        /// Nome da localidade
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// UF da localidade
        /// </summary>
        public string Uf { get; set; }
    }
}
