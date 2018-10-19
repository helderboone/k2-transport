namespace K2.Web.Models
{
    /// <summary>
    /// Comando utilizado para a alteração dos dados do usuário logado
    /// </summary>
    public class AlterarMeusDadosEntrada : BaseModel
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public string Celular { get; set; }

        public string Cnh { get; set; }
    }
}
