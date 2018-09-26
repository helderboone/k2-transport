using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios
{
    public interface IProprietarioCarroRepositorio
    {
        /// <summary>
        /// Obtém um proprietário a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<ProprietarioCarro> ObterPorId(int id, bool habilitarTracking = false);

        /// <summary>
        /// Verifica se existe um proprietário com o ID informado
        /// </summary>
        Task<bool> VerificarExistenciaPorId(int id);

        /// <summary>
        /// Verifica se existe um proprietário com o ID do usuário informado
        /// </summary>
        Task<bool> VerificarExistenciaPorIdUsuario(int id);

        /// <summary>
        /// Obtém os proprietários baseados nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarProprietarioCarroEntrada entrada);

        /// <summary>
        /// Insere um novo proprietário
        /// </summary>
        Task Inserir(ProprietarioCarro proprietario);

        /// <summary>
        /// Deleta um proprietário
        /// </summary>
        void Deletar(ProprietarioCarro proprietario);
    }
}
