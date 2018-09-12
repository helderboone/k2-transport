using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios
{
    public interface IClienteRepositorio
    {
        /// <summary>
        /// Insere uma novo cliente
        /// </summary>
        Task Inserir(Cliente cliente);

        /// <summary>
        /// Obtém os clientes baseados nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarClienteEntrada entrada);
    }
}
