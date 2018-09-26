namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para o cadastro de um motorista
    /// </summary>
    public class CadastrarMotoristaEntrada : CadastrarUsuarioEntrada
    {
        public string Cnh { get; set; }

        public CadastrarMotoristaEntrada()
        {
            base.Senha = "k2";
        }
    }
}
