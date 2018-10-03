using K2.Dominio;
using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace K2.Infraestrutura.Dados.Repositorios
{
    public class ReservaRepositorio : IReservaRepositorio
    {
        private readonly EfDataContext _efContext;

        public ReservaRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Reserva> ObterPorId(int id, bool habilitarTracking = false)
        {
            var query = _efContext.Reservas
                .Include(x => x.Cliente).ThenInclude(x => x.Usuario)
                .Include(x => x.Viagem).ThenInclude(x => x.Carro.Proprietario.Usuario)
                .Include(x => x.Viagem).ThenInclude(x => x.Motorista.Usuario)
                .Include(x => x.Viagem).ThenInclude(x => x.LocalidadeEmbarque)
                .Include(x => x.Viagem).ThenInclude(x => x.LocalidadeDesembarque)
                .Include(x => x.Dependente)
                .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Reserva>> ObterPorIdVigem(int idViagem)
        {
            return await _efContext.Reservas
               .Include(x => x.Cliente).ThenInclude(x => x.Usuario)
               .Include(x => x.Viagem).ThenInclude(x => x.Carro.Proprietario.Usuario)
               .Include(x => x.Viagem).ThenInclude(x => x.Motorista.Usuario)
               .Include(x => x.Viagem).ThenInclude(x => x.LocalidadeEmbarque)
               .Include(x => x.Viagem).ThenInclude(x => x.LocalidadeDesembarque)
               .Include(x => x.Dependente)
               .Where(x => x.IdViagem == idViagem)
               .ToListAsync();
        }

        public async Task<bool> VerificarExistenciaPorId(int id)
        {
            return await _efContext.Reservas.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> VerificarExistenciaPorClienteViagem(int idCliente, int idViagem, int? idReserva = null)
        {
            return idReserva.HasValue
                ? await _efContext.Reservas.AnyAsync(x => x.Id != idReserva && x.IdCliente == idCliente && x.IdViagem == idViagem)
                : await _efContext.Reservas.AnyAsync(x => x.IdCliente == idCliente && x.IdViagem == idViagem);
        }

        public async Task<ProcurarSaida> Procurar(ProcurarReservaEntrada entrada, CredencialUsuarioEntrada credencial)
        {
            var query = _efContext.Reservas
                .Include(x => x.Cliente).ThenInclude(x => x.Usuario)
                .Include(x => x.Viagem).ThenInclude(x => x.Carro.Proprietario.Usuario)
                .Include(x => x.Viagem).ThenInclude(x => x.Motorista.Usuario)
                .Include(x => x.Viagem).ThenInclude(x => x.LocalidadeEmbarque)
                .Include(x => x.Viagem).ThenInclude(x => x.LocalidadeDesembarque)
                .Include(x => x.Dependente)
                .AsNoTracking()
                .AsQueryable();

            switch (credencial.PerfilUsuario)
            {
                case TipoPerfil.Motorista:
                    // Quando a procura é feita por um motorista, somente as reservas cuja viagem ele será o motorista devem ser retornadas.
                    query = query.Where(x => x.Viagem.Motorista.IdUsuario == credencial.IdUsuario);
                    break;
                case TipoPerfil.ProprietarioCarro:
                    // Quando a procura é feita por um proprietário, somente as reservas cuja viagem está associada a um de seus carros devem ser retornadas.
                    query = query.Where(x => x.Viagem.Carro.Proprietario.IdUsuario == credencial.IdUsuario);
                    break;
            }

            if (entrada.IdViagem.HasValue)
                query = query.Where(x => x.IdViagem == entrada.IdViagem.Value);

            if (entrada.IdCliente.HasValue)
                query = query.Where(x => x.IdCliente == entrada.IdCliente.Value);

            if (entrada.ValorPago.HasValue)
                query = query.Where(x => x.ValorPago == entrada.ValorPago.Value);

            if (!string.IsNullOrEmpty(entrada.Observacao))
                query = query.Where(x => x.Observacao.IndexOf(entrada.Observacao, StringComparison.InvariantCultureIgnoreCase) != -1);

            query = query.OrderByProperty(entrada.OrdenarPor, entrada.OrdenarSentido);

            if (entrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(entrada.PaginaIndex.Value, entrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.AsEnumerable().Select(x => new ReservaSaida(x)),
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
                    (await query.ToListAsync()).Select(x => new ReservaSaida(x)),
                    entrada.OrdenarPor,
                    entrada.OrdenarSentido,
                    totalRegistros);
            }
        }

        public async Task Inserir(Reserva reserva)
        {
            await _efContext.AddAsync(reserva);
        }

        public void Deletar(Reserva reserva)
        {
            _efContext.Reservas.Remove(reserva);
        }
    }
}
