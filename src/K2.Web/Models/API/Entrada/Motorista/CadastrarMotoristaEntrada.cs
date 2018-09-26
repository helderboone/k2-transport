namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para o cadastro de um motorista
    /// </summary>
    public class CadastrarMotoristaEntrada : CadastrarUsuarioEntrada
    {
        /// <summary>
        /// Número da CNH do motorista
        /// </summary>
        public string Cnh { get; set; }

        public CadastrarMotoristaEntrada()
        {
            base.Senha = "k2";
        }
    }
}
