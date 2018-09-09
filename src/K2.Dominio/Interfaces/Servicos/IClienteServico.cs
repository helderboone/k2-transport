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
        /// Realiza o cadastro de um novo cliente.
        /// </summary>
        Task<ISaida> CadastrarCliente(CadastrarClienteEntrada cadastrarClienteEntrada);
    }
}
