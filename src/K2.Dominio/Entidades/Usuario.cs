using K2.Dominio.Comandos.Entrada;
using NETCore.Encrypt.Extensions;

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
        /// Perfil de acesso do usuário
        /// </summary>
        public string Perfil { get; internal set; }

        private Usuario()
        {

        }

        public Usuario (CadastrarUsuarioEntrada cadastrarEntrada)
        {
            if (cadastrarEntrada.Invalido)
                return;

            this.Nome          = cadastrarEntrada.Nome;
            this.Senha         = cadastrarEntrada.Senha.MD5();
            this.Email         = cadastrarEntrada.Email;
            this.Cpf           = cadastrarEntrada.Cpf;
            this.Rg            = cadastrarEntrada.Rg;
            this.Celular       = cadastrarEntrada.Celular;
            this.Ativo         = true;
            this.Administrador = cadastrarEntrada.Perfil == TipoPerfil.Administrador;
            this.Perfil        = cadastrarEntrada.Perfil;
        }

        public override string ToString()
        {
            return this.Nome.ToUpper();
        }
    }
}
