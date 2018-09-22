using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Localidade"
    /// </summary>
    public interface ILocalidadeServico
    {
        /// <summary>
        /// Obtém uma localidade a partir do seu ID
        /// </summary>
        Task<ISaida> ObterLocalidadePorId(int id);

        /// <summary>
        /// Obtém as localidades baseadas nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarLocalidades(ProcurarLocalidadeEntrada entrada);

        /// <summary>
        /// Realiza o cadastro de uma nova localidade.
        /// </summary>
        Task<ISaida> CadastrarLocalidade(CadastrarLocalidadeEntrada entrada);

        /// <summary>
        /// Realiza a alteração de uma localidade.
        /// </summary>
        Task<ISaida> AlterarLocalidade(AlterarLocalidadeEntrada entrada);

        /// <summary>
        /// Exclui uma localidade a partir do seu ID
        /// </summary>
        Task<ISaida> ExcluirLocalidade(int id);
    }
}
