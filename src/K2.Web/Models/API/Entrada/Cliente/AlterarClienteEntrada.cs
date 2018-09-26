namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para alteração de um cliente
    /// </summary>
    public class AlterarClienteEntrada : AlterarUsuarioEntrada
    {
        public int Id { get; set; }

        public string Cep { get; set; }

        public string Endereco { get; set; }

        public string Municipio { get; set; }

        public string Uf { get; set; }
    }
}
