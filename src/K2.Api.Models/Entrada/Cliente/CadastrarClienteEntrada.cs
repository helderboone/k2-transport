namespace K2.Api.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para o cadastro de um cliente
    /// </summary>
    public class CadastrarClienteEntrada : CadastrarUsuarioEntrada
    {
        /// <summary>
        /// CEP do cliente
        /// </summary>
        public string Cep { get; set; }

        /// <summary>
        /// Endereço do cliente
        /// </summary>
        public string Endereco { get; set; }

        /// <summary>
        /// Município do cliente
        /// </summary>
        public string Municipio { get; set; }

        /// <summary>
        /// UF do cliente
        /// </summary>
        public string Uf { get; set; }
    }
}
