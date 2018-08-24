using K2.Infraestrutura.Logging;
using K2.Infraestrutura.Logging.Providers.Database;
using System.Threading.Tasks;

namespace K2.Infraestrutura.Dados.Repositorios
{
    public class LogRepositorio : ILogRepositorio
    {
        private readonly LogDataContext _efContext;

        public LogRepositorio(LogDataContext efContext)
        {
            _efContext = efContext;
        }

        public async Task Inserir(RegistroLog item)
        {
            try
            {
                _efContext.Add(item);
            }
            catch (System.Exception ex)
            {

                throw;
            }

            await _efContext.SaveChangesAsync();
        }
    }
}
