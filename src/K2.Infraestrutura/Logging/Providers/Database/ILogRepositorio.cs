using System.Threading.Tasks;

namespace K2.Infraestrutura.Logging.Providers.Database
{
    public interface ILogRepositorio
    {
        Task Inserir(RegistroLog item);
    }
}
