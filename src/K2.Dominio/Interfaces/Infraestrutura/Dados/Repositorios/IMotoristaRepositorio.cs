using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios
{
    public interface IMotoristaRepositorio
    {
        /// <summary>
        /// Obtém um motorista a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Motorista> ObterPorId(int id, bool habilitarTracking = false);

        /// <summary>
        /// Verifica se existe um motorista com o ID informado
        /// </summary>
        Task<bool> VerificarExistenciaPorId(int id);

        /// <summary>
        /// Verifica se existe um motorista com o ID do usuário informado
        /// </summary>
        Task<bool> VerificarExistenciaPorIdUsuario(int id);

        /// <summary>
        /// Obtém os motoristas baseados nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarMotoristaEntrada entrada);

        /// <summary>
        /// Insere um novo motorista
        /// </summary>
        Task Inserir(Motorista motorista);

        /// <summary>
        /// Deleta um motorista
        /// </summary>
        void Deletar(Motorista motorista);
    }
}
