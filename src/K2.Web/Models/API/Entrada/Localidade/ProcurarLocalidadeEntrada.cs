namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para realizar a procura por localidades
    /// </summary>
    public class ProcurarLocalidadeEntrada : ProcurarEntrada
    {
        public string Nome { get; set; }

        public string Uf { get; set; }
    }
}
