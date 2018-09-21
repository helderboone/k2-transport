using JNogueira.Infraestrutura.NotifiqueMe;
using JNogueira.Infraestrutura.Utilzao;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Resources;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para a alteração de um usuário
    /// </summary>
    public class AlterarUsuarioEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; }

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
        /// Número do RG do usuário
        /// </summary>
        public string Rg { get; }

        /// <summary>
        /// Número do celular do usuário
        /// </summary>
        public string Celular { get; }

        /// <summary>
        /// Indica se o usuário está ativo
        /// </summary>
        public bool Ativo { get; }

        /// <summary>
        /// Indica que o usuário é um administrador
        /// </summary>
        public bool Administrador { get; }

        public AlterarUsuarioEntrada(int idUsuario, string nome, string email, string cpf, string rg, string celular, bool ativo, bool administrador)
        {
            IdUsuario     = idUsuario;
            Nome          = nome?.ToUpper();
            Email         = email?.ToLower();
            Cpf           = cpf?.RemoverCaracter(".", "-", "/");
            Rg            = rg?.ToUpper().RemoverCaracter(".", "-", "/");
            Celular       = celular?.RemoverCaracter("(", "-", ")");
            Ativo         = ativo;
            Administrador = administrador;

            Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, UsuarioResource.Id_Invalido)
                .NotificarSeNuloOuVazio(this.Nome, UsuarioResource.Nome_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.Email, UsuarioResource.Email_Obrigatorio_Nao_Informado)
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
