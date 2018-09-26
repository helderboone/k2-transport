using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Carro"
    /// </summary>
    public interface ICarroServico
    {
        /// <summary>
        /// Obtém um carro a partir do seu ID
        /// </summary>
        Task<ISaida> ObterCarroPorId(int id, CredencialUsuarioEntrada credencial);

        /// <summary>
        /// Obtém os carros baseadas nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarCarros(ProcurarCarroEntrada entrada, CredencialUsuarioEntrada credencial);

        /// <summary>
        /// Realiza o cadastro de um novo carro.
        /// </summary>
        Task<ISaida> CadastrarCarro(CadastrarCarroEntrada entrada);

        /// <summary>
        /// Realiza a alteração de um carro.
        /// </summary>
        Task<ISaida> AlterarCarro(AlterarCarroEntrada entrada, CredencialUsuarioEntrada credencial);

        /// <summary>
        /// Exclui um carro a partir do seu ID
        /// </summary>
        Task<ISaida> ExcluirCarro(int id, CredencialUsuarioEntrada credencial);
    }
}
