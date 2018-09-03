using K2.Dominio.Comandos.Entrada.Usuario;
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
        /// Realiza a autenticação de um usuário
        /// </summary>
        Task<ISaida> Autenticar(AutenticarUsuarioEntrada autenticacaoEntrada);

        /// <summary>
        /// Realiza a alteração da senha do usuário
        /// </summary>
        Task<ISaida> AlterarSenha(AlterarSenhaUsuarioEntrada alterarSenhaEntrada);
    }
}
