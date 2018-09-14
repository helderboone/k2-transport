namespace K2.Api.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para alteração de um usuário
    /// </summary>
    public class AlterarUsuarioEntrada : BaseModel
    {
        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; set; }

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

        /// <summary>
        /// Número do celular do usuário
        /// </summary>
        public string Celular { get; set; }

        /// <summary>
        /// Indica se o usuário está ativo
        /// </summary>
        public bool Ativo { get; set; }
    }
}
