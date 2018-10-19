using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Motorista"
    /// </summary>
    public interface IMotoristaServico
    {
        /// <summary>
        /// Obtém um motorista a partir do seu ID
        /// </summary>
        Task<ISaida> ObterMotoristaPorId(int id);

        /// <summary>
        /// Obtém um motorista a partir do ID do seu usuário
        /// </summary>
        Task<ISaida> ObterMotoristaPorIdUsuario(int idUsuario);

        /// <summary>
        /// Obtém os motoristas baseados nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarMotoristas(ProcurarMotoristaEntrada entrada);

        /// <summary>
        /// Realiza o cadastro de um novo motorista.
        /// </summary>
        Task<ISaida> CadastrarMotorista(CadastrarMotoristaEntrada entrada);

        /// <summary>
        /// Realiza a alteração de um motorista.
        /// </summary>
        Task<ISaida> AlterarMotorista(AlterarMotoristaEntrada entrada);

        /// <summary>
        /// Exclui um motorista a partir do seu ID
        /// </summary>
        Task<ISaida> ExcluirMotorista(int id);
    }
}
