namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para o cadastro de um administrador
    /// </summary>
    public class CadastrarUsuarioEntrada : BaseModel
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public string Celular { get; set; }

        public bool Administrador { get; set; }
    }
}
