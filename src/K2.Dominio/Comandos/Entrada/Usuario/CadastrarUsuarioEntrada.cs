using JNogueira.Infraestrutura.NotifiqueMe;
using JNogueira.Infraestrutura.Utilzao;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Resources;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de um usuário
    /// </summary>
    public class CadastrarUsuarioEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Senha de acesso do usuário
        /// </summary>
        public string Senha { get; }

        /// <summary>
        /// CPF do usuário
        /// </summary>
        public string Cpf { get; }

        /// <summary>
        /// Número do RG do usuário
        /// </summary>
        public string Rg { get; }

        /// <summary>
        /// Número do celular do usuário
        /// </summary>
        public string Celular { get; }

        /// <summary>
        /// Indica que o usuário é um administrador
        /// </summary>
        public bool Administrador { get; }

        public CadastrarUsuarioEntrada(string nome, string email, string senha, string cpf, string rg, string celular, bool administrador = false)
        {
            Nome          = nome?.ToUpper();
            Email         = email?.ToLower();
            Senha         = senha;
            Cpf           = cpf?.RemoverCaracter(".", "-", "/");
            Rg            = rg?.ToUpper().RemoverCaracter(".", "-", "/");
            Celular       = celular?.RemoverCaracter("(", "-", ")");
            Administrador = administrador;

            Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeNuloOuVazio(this.Nome, UsuarioResource.Nome_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.Email, UsuarioResource.Email_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.Senha, UsuarioResource.Senha_Obrigatoria_Nao_Informada)
                .NotificarSeNuloOuVazio(this.Cpf, UsuarioResource.Cpf_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.Rg, UsuarioResource.Rg_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.Celular, UsuarioResource.Celular_Obrigatorio_Nao_Informado);

            if (!string.IsNullOrEmpty(this.Email))
                this.NotificarSeEmailInvalido(this.Email, UsuarioResource.Email_Invalido);

            if (!string.IsNullOrEmpty(this.Cpf))
                this.NotificarSeFalso(this.Cpf.ValidarCpf(), UsuarioResource.Cpf_Invalido);
        }
    }
}
