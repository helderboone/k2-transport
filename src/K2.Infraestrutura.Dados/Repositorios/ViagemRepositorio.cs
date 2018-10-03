using K2.Dominio;
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
    public class ViagemRepositorio : IViagemRepositorio
    {
        private readonly EfDataContext _efContext;

        public ViagemRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Viagem> ObterPorId(int id, bool habilitarTracking = false)
        {
            var query = _efContext.Viagens
                .Include(x => x.Carro)
                .Include(x => x.Motorista)
                    .ThenInclude(x => x.Usuario)
                .Include(x => x.LocalidadeEmbarque)
                .Include(x => x.LocalidadeDesembarque)
                .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> VerificarExistenciaPorId(int id)
        {
            return await _efContext.Viagens.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> VerificarExistenciaPorMotoristaDataHorarioSaida(int idMotorista, DateTime dataHorarioSaida, int? idViagem = null)
        {
            return idViagem.HasValue
                ? await _efContext.Viagens.AnyAsync(x => x.Id != idViagem && x.IdMotorista == idMotorista && x.DataHorarioSaida == dataHorarioSaida)
                : await _efContext.Viagens.AnyAsync(x => x.IdMotorista == idMotorista && x.DataHorarioSaida == dataHorarioSaida);
        }

        public async Task<bool> VerificarExistenciaPorCarroDataHorarioSaida(int idCarro, DateTime dataHorarioSaida, int? idViagem = null)
        {
            return idViagem.HasValue
                ? await _efContext.Viagens.AnyAsync(x => x.Id != idViagem && x.IdCarro == idCarro && x.DataHorarioSaida == dataHorarioSaida)
                : await _efContext.Viagens.AnyAsync(x => x.IdCarro == idCarro && x.DataHorarioSaida == dataHorarioSaida);
        }

        public async Task<ProcurarSaida> Procurar(ProcurarViagemEntrada entrada, CredencialUsuarioEntrada credencial)
        {
            var query = _efContext.Viagens
                .Include(x => x.Carro)
                    .ThenInclude(x => x.Proprietario)
                .Include(x => x.Motorista)
                    .ThenInclude(x => x.Usuario)
                .Include(x => x.LocalidadeEmbarque)
                .Include(x => x.LocalidadeDesembarque)
                .AsNoTracking()
                .AsQueryable();

            switch (credencial.PerfilUsuario)
            {
                case TipoPerfil.Motorista:
                    // Quando a procura é feita por um motorista, somente seus viagens devem ser retornadas.
                    query = query.Where(x => x.Motorista.IdUsuario == credencial.IdUsuario);
                    break;
                case TipoPerfil.ProprietarioCarro:
                    // Quando a procura é feita por um proprietário, somente as viagens associadas a um de seus carros devem ser retornadas.
                    query = query.Where(x => x.Carro.Proprietario.IdUsuario == credencial.IdUsuario);
                    break;
                default:
                    if (entrada.IdCarro.HasValue)
                        query = query.Where(x => x.IdCarro == entrada.IdCarro.Value);

                    if (entrada.IdMotorista.HasValue)
                        query = query.Where(x => x.IdMotorista == entrada.IdMotorista.Value);
                    break;
            }

            if (entrada.IdLocalidadeEmbarque.HasValue)
                query = query.Where(x => x.IdLocalidadeEmbarque == entrada.IdLocalidadeEmbarque.Value);

            if (entrada.IdLocalidadeDesembarque.HasValue)
                query = query.Where(x => x.IdLocalidadeDesembarque == entrada.IdLocalidadeDesembarque.Value);

            if (!string.IsNullOrEmpty(entrada.Descricao))
                query = query.Where(x => x.Descricao.IndexOf(entrada.Descricao, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (entrada.ValorPassagem.HasValue)
                query = query.Where(x => x.ValorPassagem == entrada.ValorPassagem.Value);

            if (entrada.DataSaidaInicio.HasValue && entrada.DataSaidaFim.HasValue)
                query = query.Where(x => x.DataHorarioSaida >= entrada.DataSaidaInicio.Value && x.DataHorarioSaida <= entrada.DataSaidaFim.Value);

            query = query.OrderByProperty(entrada.OrdenarPor, entrada.OrdenarSentido);

            if (entrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(entrada.PaginaIndex.Value, entrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.AsEnumerable().Select(x => new ViagemSaida(x)),
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
                    (await query.ToListAsync()).Select(x => new ViagemSaida(x)),
                    entrada.OrdenarPor,
                    entrada.OrdenarSentido,
                    totalRegistros);
            }
        }

        public async Task Inserir(Viagem viagem)
        {
            await _efContext.AddAsync(viagem);
        }

        public void Deletar(Viagem viagem)
        {
            _efContext.Viagens.Remove(viagem);
        }
    }
}
