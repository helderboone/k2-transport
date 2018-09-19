using JNogueira.Infraestrutura.Utilzao;
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
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly EfDataContext _efContext;

        public UsuarioRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task<Usuario> ObterPorId(int id, bool habilitarTracking = false)
        {
            var query = _efContext.Usuarios.Where(x => x.Id == id);

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Usuario> ObterPorEmailSenha(string email, string senha, bool habilitarTracking = false)
        {
            var query = _efContext.Usuarios.Where(x => x.Email == email && x.Senha == senha);

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> VerificarExistenciaPorCpf(string cpf, int? idUsuario = null)
        {
            return idUsuario.HasValue
                ? await _efContext.Usuarios.AnyAsync(x => x.Id != idUsuario && x.Cpf.Equals(cpf.RemoverCaracter(".", "-", "/"), StringComparison.InvariantCultureIgnoreCase))
                : await _efContext.Usuarios.AnyAsync(x => x.Cpf.Equals(cpf.RemoverCaracter(".", "-", "/"), StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<bool> VerificarExistenciaPorEmail(string email, int? idUsuario = null)
        {
            return idUsuario.HasValue
                ? await _efContext.Usuarios.AnyAsync(x => x.Id != idUsuario && x.Email.ToLower() == email.ToLower())
                : await _efContext.Usuarios.AnyAsync(x => x.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> VerificarExistenciaPorRg(string rg, int? idUsuario = null)
        {
            return idUsuario.HasValue
                ? await _efContext.Usuarios.AnyAsync(x => x.Id != idUsuario && x.Rg.Equals(rg.RemoverCaracter(".", "-", "/"), StringComparison.InvariantCultureIgnoreCase))
                : await _efContext.Usuarios.AnyAsync(x => x.Rg.Equals(rg.RemoverCaracter(".", "-", "/"), StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<ProcurarSaida> Procurar(ProcurarUsuarioEntrada entrada)
        {
            var query = _efContext.Usuarios
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(entrada.Nome))
                query = query.Where(x => x.Nome.IndexOf(entrada.Nome, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(entrada.Email))
                query = query.Where(x => x.Email.IndexOf(entrada.Email, StringComparison.InvariantCultureIgnoreCase) != -1);

            if (!string.IsNullOrEmpty(entrada.Cpf))
                query = query.Where(x => x.Cpf == entrada.Cpf);

            if (!string.IsNullOrEmpty(entrada.Rg))
                query = query.Where(x => x.Rg.IndexOf(entrada.Rg, StringComparison.InvariantCultureIgnoreCase) != -1);

            query = query.OrderByProperty(entrada.OrdenarPor, entrada.OrdenarSentido);

            if (entrada.Paginar())
            {
                var pagedList = await query.ToPagedListAsync(entrada.PaginaIndex.Value, entrada.PaginaTamanho.Value);

                return new ProcurarSaida(
                    pagedList.ToList().Select(x => new UsuarioSaida(x)),
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
                    (await query.ToListAsync()).Select(x => new UsuarioSaida(x)),
                    entrada.OrdenarPor,
                    entrada.OrdenarSentido,
                    totalRegistros);
            }
        }

        public async Task Inserir(Usuario usuario)
        {
            await this._efContext.AddAsync(usuario);
        }

        public void Deletar(Usuario usuario)
        {
            _efContext.Usuarios.Remove(usuario);
        }
    }
}
