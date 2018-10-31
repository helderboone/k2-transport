using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios
{
    public interface ILogRepositorio
    {
        /// <summary>
        /// Obtém um registro do log a partir do seu ID
        /// </summary>
        Task<Log> ObterPorId(int id);

        /// <summary>
        /// Obtém os registros de log baseados nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarLogEntrada entrada);

        /// <summary>
        /// Deleta um registro do log
        /// </summary>
        Task Deletar(int id);

        /// <summary>
        /// Delete todos os registros do log
        /// </summary>
        Task Limpar();
    }
}
