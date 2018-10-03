using K2.Dominio.Entidades;
using K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace K2.Infraestrutura.Dados.Repositorios
{
    public class ReservaDependenteRepositorio : IReservaDependenteRepositorio
    {
        private readonly EfDataContext _efContext;

        public ReservaDependenteRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<ReservaDependente> ObterPorIdReserva(int idReserva, bool habilitarTracking = false)
        {
            var query = _efContext.ReservaDependentes
                .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.IdReserva == idReserva);
        }

        public async Task<bool> VerificarExistenciaPorReserva(int idReserva)
        {
            return await _efContext.ReservaDependentes.AnyAsync(x => x.IdReserva == idReserva);
        }

        public async Task Inserir(ReservaDependente dependente)
        {
            await _efContext.AddAsync(dependente);
        }

        public void Deletar(ReservaDependente dependente)
        {
            _efContext.ReservaDependentes.Remove(dependente);
        }
    }
}
