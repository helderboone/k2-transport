using K2.Dominio.Entidades;
using K2.Dominio.Interfaces.Dados.Repositorios;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace K2.Infraestrutura.Dados.Repositorios
{
    public class UsuarioRepositorio : IUsuarioRepositorio
    {
        private readonly EfDataContext _efContext;

        public UsuarioRepositorio(EfDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task Inserir(Usuario usuario)
        {
            await this._efContext.AddAsync(usuario);
        }

        public async Task<Usuario> ObterPorEmailSenha(string email, string senha, bool habilitarTracking = false)
        {
            var query = _efContext.Usuarios.Where(x => x.Email == email && x.Senha == senha);

            if (!habilitarTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> VerificarExistenciaPorEmail(string email, int? idUsuario = null)
        {
            return idUsuario.HasValue
                ? await _efContext.Usuarios.AnyAsync(x => x.Id != idUsuario && x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
                : await _efContext.Usuarios.AnyAsync(x => x.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
