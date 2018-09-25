using JNogueira.Infraestrutura.NotifiqueMe;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando para entrada das crendenciais do usuário. Essas credenciais devem ser utilizadas em métodos que dependendo do perfil do usuário deve desempenhar ações diferentes.
    /// </summary>
    public class CredencialUsuarioEntrada : Notificavel
    {
        public int IdUsuario { get; }

        public string PerfilUsuario { get; }

        public CredencialUsuarioEntrada(int idUsuario, string perfilUsuario)
        {
            this.IdUsuario     = idUsuario;
            this.PerfilUsuario = perfilUsuario;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, "Na credencial do usuário, o ID é inválido.")
                .NotificarSeNuloOuVazio(this.PerfilUsuario, "Na credencial do usuário, o perfil é obrigatório e não foi informado.");
        }
    }
}
