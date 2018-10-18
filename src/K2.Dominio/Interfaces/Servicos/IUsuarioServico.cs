using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Usuário"
    /// </summary>
    public interface IUsuarioServico
    {
        /// <summary>
        /// Obtém um usuário a partir do seu ID
        /// </summary>
        Task<ISaida> ObterUsuarioPorId(int id);

        /// <summary>
        /// Realiza a autenticação de um usuário
        /// </summary>
        Task<ISaida> Autenticar(AutenticarUsuarioEntrada entrada);

        /// <summary>
        /// Realiza a alteração da senha do usuário
        /// </summary>
        Task<ISaida> AlterarSenha(AlterarSenhaUsuarioEntrada entrada);

        /// <summary>
        /// Redefine a senha do usuário para uma senha temporária
        /// </summary>
        Task<ISaida> RedefinirSenha(int id);

        /// <summary>
        /// Redefine a senha do usuário para uma senha temporária
        /// </summary>
        Task<ISaida> RedefinirSenha(string email);

        /// <summary>
        /// Obtém os usuários baseadas nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarUsuarios(ProcurarUsuarioEntrada entrada);

        /// <summary>
        /// Realiza o cadastro de um novo usuario.
        /// </summary>
        Task<ISaida> CadastrarUsuario(CadastrarUsuarioEntrada entrada);

        /// <summary>
        /// Realiza a alteração de um usuario.
        /// </summary>
        Task<ISaida> AlterarUsuario(AlterarUsuarioEntrada entrada);

        /// <summary>
        /// Exclui um usuario a partir do seu ID
        /// </summary>
        Task<ISaida> ExcluirUsuario(int id);
    }
}
