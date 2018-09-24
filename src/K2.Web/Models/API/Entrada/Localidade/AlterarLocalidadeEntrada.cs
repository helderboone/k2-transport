namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para alteração de uma localidade
    /// </summary>
    public class AlterarLocalidadeEntrada : BaseModel
    {
        /// <summary>
        /// ID da localidade
        /// </summary>
        public int Id { get; set; }

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
