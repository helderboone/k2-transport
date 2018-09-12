namespace K2.Api.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para realizar a procura por clientes
    /// </summary>
    public class ProcurarClienteEntrada : ProcurarEntrada
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// CPF do usuário
        /// </summary>
        public string Cpf { get; set; }

        /// <summary>
        /// Número do RG do usuário
        /// </summary>
        public string Rg { get; set; }

        public ProcurarClienteEntrada()
        {
            this.OrdenarPor = "Nome";
        }
    }
}
