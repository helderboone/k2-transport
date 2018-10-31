using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Log"
    /// </summary>
    public interface ILogServico
    {
        /// <summary>
        /// Obtém um registro do log a partir do seu ID
        /// </summary>
        Task<ISaida> ObterRegistroPorId(int id);

        /// <summary>
        /// Obtém os registros do log baseados nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarRegistros(ProcurarLogEntrada entrada);

        /// <summary>
        /// Exclui um registro do log a partir do seu ID
        /// </summary>
        Task ExcluirRegistro(int id);

        /// <summary>
        /// Exclui todos os registros do log
        /// </summary>
        Task LimparLog();
    }
}
