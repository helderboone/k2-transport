using K2.Dominio.Entidades;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios
{
    public interface IUsuarioRepositorio
    {
        /// <summary>
        /// Obtém um usuário a partir do seu e-mail e senha
        /// </summary>
        Task<Usuario> ObterPorEmailSenha(string email, string senha, bool habilitarTracking = false);

        /// <summary>
        /// Verifica se existe um usuário com o mesmo e-mail
        /// </summary>
        Task<bool> VerificarExistenciaPorEmail(string email, int? idUsuario = null);

        /// <summary>
        /// Verifica se existe um usuário com o mesmo CPF
        /// </summary>
        Task<bool> VerificarExistenciaPorCpf(string cpf, int? idUsuario = null);

        /// <summary>
        /// Verifica se existe um usuário com o mesmo RG
        /// </summary>
        Task<bool> VerificarExistenciaPorRg(string rg, int? idUsuario = null);

        /// <summary>
        /// Insere uma novo usuário
        /// </summary>
        Task Inserir(Usuario usuario);
    }
}
