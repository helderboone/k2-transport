namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para alteração de um usuário
    /// </summary>
    public class AlterarUsuarioEntrada : BaseModel
    {
        public int IdUsuario { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public string Celular { get; set; }

        public bool Ativo { get; set; }

        public bool Administrador { get; }

        public AlterarUsuarioEntrada()
        {
            this.Administrador = true;
        }
    }
}
