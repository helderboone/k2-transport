using K2.Dominio.Entidades;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Dados.Repositorios
{
    public interface IUsuarioRepositorio
    {
        /// <summary>
        /// Obtém um usuário a partir do seu e-mail e senha
        /// </summary>
        Task<Usuario> ObterPorEmailSenha(string email, string senha, bool habilitarTracking = false);
    }
}
