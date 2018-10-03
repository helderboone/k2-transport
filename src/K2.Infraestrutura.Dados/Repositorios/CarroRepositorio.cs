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
    public class CarroRepositorio : ICarroRepositorio
    {
        private readonly EfDataContext _efContext;

        public CarroRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Carro> ObterPorId(int id, bool habilitarTracking = false)
        {
            var query = _efContext.Carros
                    .Include(x => x.Proprietario)
                    .ThenInclude(x => x.Usuario)
                    .AsQueryable();

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> VerificarExistenciaPorId(int id)
        {
            return await _efContext.Carros.AnyAsync(x => x.Id == id);
        }

        public async Task<ProcurarSaida> Procurar(ProcurarCarroEntrada entrada, CredencialUsuarioEntrada credencial)
        {
            var query = _efContext.Carros
                .Include(x => x.Proprietario)
                .ThenInclude(x => x.Usuario)
                .AsNoTracking()
                .AsQueryable();

            switch (credencial.PerfilUsuario)
            {
                case TipoPerfil.ProprietarioCarro:
                    // Quando a procura é feita por um proprietário, somente seus carros devem ser retornados.
                    query = query.Where(x => x.Proprietario.IdUsuario == credencial.IdUsuario);
                    break;
                default:
                    if (entrada.IdProprietario.HasValue)
                        query = query.Where(x => x.IdProprietario == entrada.IdProprietario.Value);
                    break;
            }

            if (!string.IsNullOrEmpty(entrada.Descricao))
                query = query.Where(x => x.Descricao.IndexOf(entrada.Descricao, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(entrada.NomeFabricante))
                query = query.Where(x => x.NomeFabricante.IndexOf(entrada.NomeFabricante, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(entrada.AnoModelo))
                query = query.Where(x => x.AnoModelo.IndexOf(entrada.AnoModelo, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (entrada.QuantidadeLugares.HasValue)
                query = query.Where(x => x.QuantidadeLugares == entrada.QuantidadeLugares.Value);

            if (!string.IsNullOrEmpty(entrada.Placa))
                query = query.Where(x => x.Placa.IndexOf(entrada.Placa, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(entrada.Renavam))
                query = query.Where(x => x.Renavam.IndexOf(entrada.Renavam, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(entrada.Caracteristicas))
                query = query.Where(x => x.Caracteristicas.IndexOf(entrada.Caracteristicas, StringComparison.InvariantCultureIgnoreCase) != -1);

            query = query.OrderByProperty(entrada.OrdenarPor, entrada.OrdenarSentido);

            if (entrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(entrada.PaginaIndex.Value, entrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.AsEnumerable().Select(x => new CarroSaida(x)),
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
                    (await query.ToListAsync()).Select(x => new CarroSaida(x)),
                    entrada.OrdenarPor,
                    entrada.OrdenarSentido,
                    totalRegistros);
            }
        }

        public async Task Inserir(Carro carro)
        {
            await _efContext.AddAsync(carro);
        }

        public void Deletar(Carro carro)
        {
            _efContext.Carros.Remove(carro);
        }
    }
}
