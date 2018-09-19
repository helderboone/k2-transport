namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para alteração de um motorista
    /// </summary>
    public class AlterarMotoristaEntrada : AlterarUsuarioEntrada
    {
        /// <summary>
        /// ID do motorista
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Número da CNH do motorista
        /// </summary>
        public string Cnh { get; set; }
    }
}
