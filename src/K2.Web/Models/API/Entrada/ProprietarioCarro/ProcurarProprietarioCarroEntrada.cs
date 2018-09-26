namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para realizar a procura por proprietários
    /// </summary>
    public class ProcurarProprietarioCarroEntrada : ProcurarEntrada
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }
    }
}
