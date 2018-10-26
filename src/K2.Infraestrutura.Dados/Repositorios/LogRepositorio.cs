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
    public class LogRepositorio : ILogRepositorio
    {
        private readonly EfDataContext _efContext;

        public LogRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Log> ObterPorId(int id) => await _efContext.Log.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<ProcurarSaida> Procurar(ProcurarLogEntrada entrada)
        {
            var query = _efContext.Log
                .AsNoTracking()
                .AsQueryable();

            if (entrada.DataInicio.HasValue && entrada.DataFim.HasValue)
                query = query.Where(x => x.Data >= entrada.DataInicio.Value && x.Data <= entrada.DataFim.Value);

            if (!string.IsNullOrEmpty(entrada.NomeOrigem))
                query = query.Where(x => x.NomeOrigem.IndexOf(entrada.NomeOrigem, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(entrada.Tipo))
                query = query.Where(x => x.Tipo == entrada.Tipo);

            if (!string.IsNullOrEmpty(entrada.Mensagem))
                query = query.Where(x => x.Mensagem.IndexOf(entrada.Mensagem, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(entrada.Usuario))
                query = query.Where(x => x.Usuario.IndexOf(entrada.Usuario, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(entrada.ExceptionInfo))
                query = query.Where(x => x.ExceptionInfo.IndexOf(entrada.ExceptionInfo, StringComparison.InvariantCultureIgnoreCase) != -1);

            query = query.OrderByProperty(entrada.OrdenarPor, entrada.OrdenarSentido);

            if (entrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(entrada.PaginaIndex.Value, entrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.AsEnumerable().Select(x => new LogSaida(x)),
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
                    (await query.ToListAsync()).Select(x => new LogSaida(x)),
                    entrada.OrdenarPor,
                    entrada.OrdenarSentido,
                    totalRegistros);
            }
        }
    }
}
