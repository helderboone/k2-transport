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
    public class ProprietarioCarroRepositorio : IProprietarioCarroRepositorio
    {
        private readonly EfDataContext _efContext;

        public ProprietarioCarroRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<ProprietarioCarro> ObterPorId(int id, bool habilitarTracking = false)
        {
            var query = _efContext.Proprietarios
                    .Include(x => x.Usuario)
                    .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> VerificarExistenciaPorId(int id)
        {
            return await _efContext.Proprietarios.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> VerificarExistenciaPorIdUsuario(int id)
        {
            return await _efContext.Proprietarios.AnyAsync(x => x.IdUsuario == id);
        }

        public async Task<ProcurarSaida> Procurar(ProcurarProprietarioCarroEntrada entrada)
        {
            var query = _efContext.Proprietarios
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
                query = query.Where(x => x.Usuario.Rg.IndexOf(entrada.Rg, StringComparison.InvariantCultureIgnoreCase) != -1);


            switch (entrada.OrdenarPor?.ToLower())
            {
                case "nome":
                    query = entrada.OrdenarSentido.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderBy(x => x.Usuario.Nome) : query.OrderByDescending(x => x.Usuario.Nome);
                    break;
                case "email":
                    query = entrada.OrdenarSentido.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderBy(x => x.Usuario.Email) : query.OrderByDescending(x => x.Usuario.Email);
                    break;
                case "cpf":
                    query = entrada.OrdenarSentido.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderBy(x => x.Usuario.Cpf) : query.OrderByDescending(x => x.Usuario.Cpf);
                    break;
                case "rg":
                    query = entrada.OrdenarSentido.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderBy(x => x.Usuario.Rg) : query.OrderByDescending(x => x.Usuario.Rg);
                    break;
                default:
                    query = query.OrderByProperty(entrada.OrdenarPor, entrada.OrdenarSentido);
                    break;
            }

            if (entrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(entrada.PaginaIndex.Value, entrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.ToList().Select(x => new ProprietarioCarroSaida(x)),
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
                    (await query.ToListAsync()).Select(x => new ProprietarioCarroSaida(x)),
                    entrada.OrdenarPor,
                    entrada.OrdenarSentido,
                    totalRegistros);
            }
        }

        public async Task Inserir(ProprietarioCarro proprietario)
        {
            await _efContext.AddAsync(proprietario);
        }

        public void Deletar(ProprietarioCarro proprietario)
        {
            _efContext.Proprietarios.Remove(proprietario);
        }
    }
}
