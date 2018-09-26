namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para alteração de um motorista
    /// </summary>
    public class AlterarMotoristaEntrada : AlterarUsuarioEntrada
    {
        public int Id { get; set; }

        public string Cnh { get; set; }
    }
}
