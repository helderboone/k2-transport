namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados na alteração da senha do usuário
    /// </summary>
    public class AlterarSenhaUsuarioEntrada : BaseModel
    {
        /// <summary>
        /// Senha atual do usuário
        /// </summary>
        public string SenhaAtual { get; set; }

        /// <summary>
        /// Nova senha do usuário
        /// </summary>
        public string SenhaNova { get; set; }

        /// <summary>
        /// Confirmação da nova senha do usuário
        /// </summary>
        public string ConfirmacaoSenhaNova { get; set; }

        /// <summary>
        /// Indica se um e-mail com a nova senha deverá ser enviado ao usuário
        /// </summary>
        public bool EnviarEmailSenhaNova { get; set; }
    }
}
