using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios;
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

        public async Task<ProcurarSaida> Procurar(ProcurarClienteEntrada entrada)
        {
            var query = _efContext.Clientes
                .Include(x => x.Usuario)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(entrada.Nome))
                query = query.Where(x => x.Usuario.Nome.IndexOf(entrada.Nome, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(entrada.Email))
                query = query.Where(x => x.Usuario.Email.IndexOf(entrada.Email, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(entrada.Cpf))
                query = query.Where(x => x.Usuario.Cpf == entrada.Cpf);

            if (!string.IsNullOrEmpty(entrada.Rg))
                query = query.Where(x => x.Usuario.Rg == entrada.Rg);

            switch (entrada.OrdenarPor)
            {
                case "Nome":
                    query = entrada.OrdenarSentido == "ASC" ? query.OrderBy(x => x.Usuario.Nome) : query.OrderByDescending(x => x.Usuario.Nome);
                    break;
                case "Email":
                    query = entrada.OrdenarSentido == "ASC" ? query.OrderBy(x => x.Usuario.Email) : query.OrderByDescending(x => x.Usuario.Email);
                    break;
                case "Cpf":
                    query = entrada.OrdenarSentido == "ASC" ? query.OrderBy(x => x.Usuario.Cpf) : query.OrderByDescending(x => x.Usuario.Cpf);
                    break;
                case "Rg":
                    query = entrada.OrdenarSentido == "ASC" ? query.OrderBy(x => x.Usuario.Rg) : query.OrderByDescending(x => x.Usuario.Rg);
                    break;
                default:
                    query = query.OrderByProperty(entrada.OrdenarPor, entrada.OrdenarSentido);
                    break;
            }

            if (entrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(entrada.PaginaIndex.Value, entrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.ToList().Select(x => new ClienteSaida(x)),
                    entrada.OrdenarPor,
                    entrada.OrdenarSentido,
                    pagedList.TotalItemCount,
                    pagedList.PageCount,
                    entrada.PaginaIndex,
                    entrada.PaginaTamanho);
            }
            else
            {
                var totalRegistros = await query.CountAsync();

                return new ProcurarSaida(
                    (await query.ToListAsync()).Select(x => new ClienteSaida(x)),
                    entrada.OrdenarPor,
                    entrada.OrdenarSentido,
                    totalRegistros);
            }
        }
    }
}
