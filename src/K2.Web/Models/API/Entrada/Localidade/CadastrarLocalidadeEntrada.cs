namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para o cadastro de uma localidade
    /// </summary>
    public class CadastrarLocalidadeEntrada : BaseModel
    {
        public string Nome { get; set; }

        public string Sigla { get; set; }

        public string Uf { get; set; }
    }
}
