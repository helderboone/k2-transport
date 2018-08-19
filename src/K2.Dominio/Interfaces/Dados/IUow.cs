using JNogueira.Infraestrutura.NotifiqueMe;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Contrato para utilização do padrão Unit Of Work
    /// </summary>
    public interface IUow : INotificavel
    {
        Task Commit();
    }
}
