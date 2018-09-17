using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Cliente"
    /// </summary>
    public interface IClienteServico
    {
        /// <summary>
        /// Obtém um cliente a partir do seu ID
        /// </summary>
        Task<ISaida> ObterClientePorId(int id);

        /// <summary>
        /// Obtém os clientes baseadas nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarClientes(ProcurarClienteEntrada entrada);

        /// <summary>
        /// Realiza o cadastro de um novo cliente.
        /// </summary>
        Task<ISaida> CadastrarCliente(CadastrarClienteEntrada entrada);

        /// <summary>
        /// Realiza a alteração de um cliente.
        /// </summary>
        Task<ISaida> AlterarCliente(AlterarClienteEntrada entrada);

        /// <summary>
        /// Exclui um cliente a partir do seu ID
        /// </summary>
        Task<ISaida> ExcluirCliente(int id);
    }
}
