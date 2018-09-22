using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios
{
    public interface ILocalidadeRepositorio
    {
        /// <summary>
        /// Obtém uma localidade a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Localidade> ObterPorId(int id, bool habilitarTracking = false);

        /// <summary>
        /// Verifica se existe uma localidade com o nome e UF informados
        /// </summary>
        Task<bool> VerificarExistenciaPorNomeUf(string nome, string uf, int? idLocalidade = null);

        /// <summary>
        /// Obtém as localidades baseadas nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarLocalidadeEntrada entrada);

        /// <summary>
        /// Insere uma nova localidade
        /// </summary>
        Task Inserir(Localidade localidade);

        /// <summary>
        /// Deleta uma localidade
        /// </summary>
        void Deletar(Localidade localidade);
    }
}
