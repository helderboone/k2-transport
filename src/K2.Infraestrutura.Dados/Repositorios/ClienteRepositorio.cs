using K2.Dominio.Entidades;
using K2.Dominio.Interfaces.Dados.Repositorios;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace K2.Infraestrutura.Dados.Repositorios
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly EfDataContext _efContext;

        public ClienteRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task Inserir(Cliente cliente)
        {
            await _efContext.AddAsync(cliente);
        }
    }
}
