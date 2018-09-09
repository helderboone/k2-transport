using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Resources;
using NETCore.Encrypt.Extensions;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado na alteração da senha de um usuário
    /// </summary>
    public class AlterarSenhaUsuarioEntrada : Notificavel, IEntrada
    {
        public string Email { get; }

        public string SenhaAtual { get; }

        public string SenhaNova { get; }

        public string ConfirmacaoSenhaNova { get; }

        public AlterarSenhaUsuarioEntrada(string email, string senhaAtual, string senhaNova, string confirmacaoSenhaNova)
        {
            this.Email                = email;
            this.SenhaAtual           = senhaAtual;
            this.SenhaNova            = senhaNova;
            this.ConfirmacaoSenhaNova = confirmacaoSenhaNova;

            this.Validar();

            if (this.Invalido)
                return;

            this.SenhaAtual = senhaAtual.MD5();
            this.SenhaNova  = senhaNova.MD5();
        }

        private void Validar()
        {
            this
                .NotificarSeNuloOuVazio(Email, UsuarioResource.Email_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(SenhaAtual, UsuarioResource.Senha_Atual_Obrigatoria_Nao_Informada)
                .NotificarSeNuloOuVazio(SenhaNova, UsuarioResource.Senha_Nova_Obrigatoria_Nao_Informada);

            if (this.Invalido)
                return;

            this
                .NotificarSeEmailInvalido(this.Email, UsuarioResource.Email_Invalido)
                .NotificarSeDiferentes(this.SenhaNova, this.ConfirmacaoSenhaNova, UsuarioResource.Senha_Confirmacao_senha_diferentes);
        }
    }
}
