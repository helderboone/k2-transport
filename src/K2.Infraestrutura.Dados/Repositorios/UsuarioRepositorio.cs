using K2.Dominio.Entidades;
using K2.Dominio.Interfaces.Dados.Repositorios;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Usuario> ObterPorEmailSenha(string email, string senha)
        {
            return await _efContext
                .Usuarios
                .Where(x => x.Email == email && x.Senha == senha)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
