namespace K2.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um usuário
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        public string Senha { get; internal set; }

        /// <summary>
        /// CPF do usuário
        /// </summary>
        public string Cpf { get; internal set; }

        /// <summary>
        /// RG do usuário
        /// </summary>
        public string Rg { get; internal set; }

        /// <summary>
        /// Celular do usuário
        /// </summary>
        public string Celular { get; internal set; }

        /// <summary>
        /// Indica se o usuário está ativo
        /// </summary>
        public bool Ativo { get; private set; }

        /// <summary>
        /// Indica se o usuário é um administrador
        /// </summary>
        public bool Administrador { get; private set; }

        /// <summary>
        /// Permissões de acesso do usuário
        /// </summary>
        public string[] PermissoesAcesso { get; internal set; }

        private Usuario()
        {

        }

        public Usuario(string nome, string email, string cpf, string rg, string celular, bool ativo = true, bool administrador = false)
            : this()
        {
            this.Nome          = nome;
            this.Email         = email;
            this.Cpf           = cpf;
            this.Rg            = rg;
            this.Celular       = celular;
            this.Ativo         = ativo;
            this.Administrador = administrador;
        }

        public override string ToString()
        {
            return this.Nome.ToUpper();
        }
    }
}
