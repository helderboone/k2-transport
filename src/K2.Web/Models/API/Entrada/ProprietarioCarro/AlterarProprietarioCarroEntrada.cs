namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para alteração de um proprietário
    /// </summary>
    public class AlterarProprietarioCarroEntrada : AlterarUsuarioEntrada
    {
        /// <summary>
        /// ID do proprietário
        /// </summary>
        public int Id { get; set; }
    }
}
