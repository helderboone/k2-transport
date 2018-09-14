namespace K2.Api.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para alteração de um cliente
    /// </summary>
    public class AlterarClienteEntrada : AlterarUsuarioEntrada
    {
        /// <summary>
        /// ID do cliente
        /// </summary>
        public int Id { get; set; }

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
