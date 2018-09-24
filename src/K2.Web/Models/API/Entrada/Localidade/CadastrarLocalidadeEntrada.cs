namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para o cadastro de uma localidade
    /// </summary>
    public class CadastrarLocalidadeEntrada : BaseModel
    {
        /// <summary>
        /// Nome da localidade
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Sigla da UF da localidade
        /// </summary>
        public string Uf { get; set; }
    }
}
