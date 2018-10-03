using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios
{
    public interface IClienteRepositorio
    {
        /// <summary>
        /// Obtém um cliente a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Cliente> ObterPorId(int id, bool habilitarTracking = false);

        /// <summary>
        /// Verifica se existe um cliente com o ID informado
        /// </summary>
        Task<bool> VerificarExistenciaPorId(int id);

        /// <summary>
        /// Verifica se existe um cliente com o ID do usuário informado
        /// </summary>
        Task<bool> VerificarExistenciaPorIdUsuario(int id);

        /// <summary>
        /// Obtém os clientes baseados nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarClienteEntrada entrada);

        /// <summary>
        /// Insere um novo cliente
        /// </summary>
        Task Inserir(Cliente cliente);

        /// <summary>
        /// Deleta um cliente
        /// </summary>
        void Deletar(Cliente cliente);
    }
}
