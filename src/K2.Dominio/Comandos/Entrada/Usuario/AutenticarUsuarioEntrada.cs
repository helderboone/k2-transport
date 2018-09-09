using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Resources;
using NETCore.Encrypt.Extensions;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado na autenticação de um usuário
    /// </summary>
    public class AutenticarUsuarioEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        public string Senha { get; }

        public AutenticarUsuarioEntrada(string email, string senha)
        {
            this.Email = email;
            this.Senha = !string.IsNullOrEmpty(senha)
                ? senha.MD5()
                : null;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeNuloOuVazio(this.Email, UsuarioResource.Email_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.Senha, UsuarioResource.Senha_Obrigatoria_Nao_Informada);

            if (this.Invalido)
                return;

            this.NotificarSeEmailInvalido(this.Email, string.Format(UsuarioResource.Email_Invalido, this.Email));
        }
    }
}
