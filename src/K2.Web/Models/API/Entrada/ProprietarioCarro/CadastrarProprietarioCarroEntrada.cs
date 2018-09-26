namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para o cadastro de um proprietário
    /// </summary>
    public class CadastrarProprietarioCarroEntrada : CadastrarUsuarioEntrada
    {
        public CadastrarProprietarioCarroEntrada()
        {
            base.Senha = "k2";
        }
    }
}
