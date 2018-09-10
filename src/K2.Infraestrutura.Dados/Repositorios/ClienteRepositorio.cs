using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using K2.Dominio.Interfaces.Dados.Repositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

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

        public async Task<ProcurarSaida> Procurar(ProcurarClienteEntrada procurarEntrada)
        {
            var query = _efContext.Clientes
                .Include(x => x.Usuario)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(procurarEntrada.Nome))
                query = query.Where(x => x.Usuario.Nome.IndexOf(procurarEntrada.Nome, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(procurarEntrada.Email))
                query = query.Where(x => x.Usuario.Email.IndexOf(procurarEntrada.Email, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(procurarEntrada.Cpf))
                query = query.Where(x => x.Usuario.Cpf == procurarEntrada.Cpf);

            if (!string.IsNullOrEmpty(procurarEntrada.Rg))
                query = query.Where(x => x.Usuario.Rg == procurarEntrada.Rg);

            query = query.OrderByProperty(procurarEntrada.OrdenarPor, procurarEntrada.OrdenarSentido);

            if (procurarEntrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(procurarEntrada.PaginaIndex.Value, procurarEntrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.ToList().Select(x => new ClienteSaida(x)),
                    procurarEntrada.OrdenarPor,
                    procurarEntrada.OrdenarSentido,
                    pagedList.TotalItemCount,
                    pagedList.PageCount,
                    procurarEntrada.PaginaIndex,
                    procurarEntrada.PaginaTamanho);
            }
            else
            {
                var totalRegistros = await query.CountAsync();

                return new ProcurarSaida(
                    (await query.ToListAsync()).Select(x => new ClienteSaida(x)),
                    procurarEntrada.OrdenarPor,
                    procurarEntrada.OrdenarSentido,
                    totalRegistros);
            }
        }
    }
}
