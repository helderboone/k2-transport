namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para alteração de uma localidade
    /// </summary>
    public class AlterarLocalidadeEntrada : BaseModel
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Uf { get; set; }
    }
}
