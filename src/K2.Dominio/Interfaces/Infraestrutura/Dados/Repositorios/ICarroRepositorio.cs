using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios
{
    public interface ICarroRepositorio
    {
        /// <summary>
        /// Obtém um carro a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Carro> ObterPorId(int id, bool habilitarTracking = false);

        /// <summary>
        /// Verifica se existe um carro com o ID informado
        /// </summary>
        Task<bool> VerificarExistenciaPorId(int id);

        /// <summary>
        /// Obtém os carros baseados nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarCarroEntrada entrada, CredencialUsuarioEntrada credencial);

        /// <summary>
        /// Insere um novo carro
        /// </summary>
        Task Inserir(Carro carro);

        /// <summary>
        /// Deleta um carro
        /// </summary>
        void Deletar(Carro carro);
    }
}
