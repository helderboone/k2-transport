using K2.Dominio.Entidades;

namespace K2.Dominio.Comandos.Saida
{
    /// <summary>
    /// Comando de sáida para as informações de um usuário
    /// </summary>
    public class UsuarioSaida
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// CPF do usuário
        /// </summary>
        public string Cpf { get; }

        /// <summary>
        /// RG do usuário
        /// </summary>
        public string Rg { get; }

        /// <summary>
        /// Celular do usuário
        /// </summary>
        public string Celular{ get; }

        /// <summary>
        /// Indica se o usuário está ativo
        /// </summary>
        public bool Ativo { get; }

        /// <summary>
        /// Indica se o usuário é um administrador
        /// </summary>
        public bool Administrador{ get; }

        /// <summary>
        /// Perfil de acesso do usuário
        /// </summary>
        public string Perfil { get; }

        public UsuarioSaida(Usuario usuario, string tipoPerfil = null)
        {
            if (usuario == null)
                return;

            this.Id            = usuario.Id;
            this.Nome          = usuario.Nome.ToUpper();
            this.Email         = usuario.Email?.ToLower();
            this.Cpf           = usuario.Cpf;
            this.Rg            = usuario.Rg;
            this.Celular       = usuario.Celular;
            this.Ativo         = usuario.Ativo;
            this.Administrador = usuario.Administrador;
            this.Perfil        = tipoPerfil;
        }

        public override string ToString()
        {
            return $"{this.Nome} - {this.Perfil}";
        }
    }
}
