using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Viagem"
    /// </summary>
    public interface IViagemServico
    {
        /// <summary>
        /// Obtém uma viagem a partir do seu ID
        /// </summary>
        Task<ISaida> ObterViagemPorId(int id, CredencialUsuarioEntrada credencial);

        /// <summary>
        /// Obtém as viagems baseadas nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarViagens(ProcurarViagemEntrada entrada, CredencialUsuarioEntrada credencial);

        /// <summary>
        /// Realiza o cadastro de uma nova viagem.
        /// </summary>
        Task<ISaida> CadastrarViagem(CadastrarViagemEntrada entrada);

        /// <summary>
        /// Realiza a alteração de uma viagem.
        /// </summary>
        Task<ISaida> AlterarViagem(AlterarViagemEntrada entrada);

        /// <summary>
        /// Exclui uma viagem a partir do seu ID
        /// </summary>
        Task<ISaida> ExcluirViagem(int id);
    }
}
