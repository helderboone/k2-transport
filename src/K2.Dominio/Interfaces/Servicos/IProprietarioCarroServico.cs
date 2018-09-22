using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "ProprietarioCarro"
    /// </summary>
    public interface IProprietarioCarroServico
    {
        /// <summary>
        /// Obtém um proprietário a partir do seu ID
        /// </summary>
        Task<ISaida> ObterProprietarioCarroPorId(int id);

        /// <summary>
        /// Obtém os proprietários baseados nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarProprietarioCarros(ProcurarProprietarioCarroEntrada entrada);

        /// <summary>
        /// Realiza o cadastro de um novo proprietário.
        /// </summary>
        Task<ISaida> CadastrarProprietarioCarro(CadastrarProprietarioCarroEntrada entrada);

        /// <summary>
        /// Realiza a alteração de um proprietário.
        /// </summary>
        Task<ISaida> AlterarProprietarioCarro(AlterarProprietarioCarroEntrada entrada);

        /// <summary>
        /// Exclui um proprietário a partir do seu ID
        /// </summary>
        Task<ISaida> ExcluirProprietarioCarro(int id);
    }
}
