using K2.Dominio.Entidades;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Dados.Repositorios
{
    public interface IClienteRepositorio
    {
        /// <summary>
        /// Insere uma novo cliente
        /// </summary>
        Task Inserir(Cliente cliente);
    }
}
