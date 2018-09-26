namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para realizar a procura por clientes
    /// </summary>
    public class ProcurarClienteEntrada : ProcurarEntrada
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }
    }
}
