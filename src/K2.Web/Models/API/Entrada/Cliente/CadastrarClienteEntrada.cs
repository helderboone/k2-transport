namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para o cadastro de um cliente
    /// </summary>
    public class CadastrarClienteEntrada : CadastrarUsuarioEntrada
    {
        public string Cep { get; set; }

        public string Endereco { get; set; }

        public string Municipio { get; set; }

        public string Uf { get; set; }
    }
}
