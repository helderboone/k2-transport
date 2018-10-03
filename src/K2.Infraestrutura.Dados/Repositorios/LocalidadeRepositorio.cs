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
    public class LocalidadeRepositorio : ILocalidadeRepositorio
    {
        private readonly EfDataContext _efContext;

        public LocalidadeRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Localidade> ObterPorId(int id, bool habilitarTracking = false)
        {
            var query = _efContext.Localidades
                    .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> VerificarExistenciaPorId(int id)
        {
            return await _efContext.Localidades.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> VerificarExistenciaPorNomeUf(string nome, string uf, int? idLocalidade = null)
        {
            return idLocalidade.HasValue
                ? await _efContext.Localidades.AnyAsync(x => x.Id != idLocalidade && x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase) && x.Uf.Equals(uf, StringComparison.InvariantCultureIgnoreCase))
                : await _efContext.Localidades.AnyAsync(x => x.Nome.Equals(nome, StringComparison.InvariantCultureIgnoreCase) && x.Uf.Equals(uf, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<ProcurarSaida> Procurar(ProcurarLocalidadeEntrada entrada)
        {
            var query = _efContext.Localidades
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(entrada.Nome))
                query = query.Where(x => x.Nome.IndexOf(entrada.Nome, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(entrada.Uf))
                query = query.Where(x => x.Uf.IndexOf(entrada.Uf, StringComparison.InvariantCultureIgnoreCase) != -1);

            query = query.OrderByProperty(entrada.OrdenarPor, entrada.OrdenarSentido);

            if (entrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(entrada.PaginaIndex.Value, entrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.AsEnumerable().Select(x => new LocalidadeSaida(x)),
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
                    (await query.ToListAsync()).Select(x => new LocalidadeSaida(x)),
                    entrada.OrdenarPor,
                    entrada.OrdenarSentido,
                    totalRegistros);
            }
        }

        public async Task Inserir(Localidade localidade)
        {
            await _efContext.AddAsync(localidade);
        }

        public void Deletar(Localidade localidade)
        {
            _efContext.Localidades.Remove(localidade);
        }

        
    }
}
