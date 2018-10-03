using K2.Dominio.Entidades;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios
{
    public interface IReservaDependenteRepositorio
    {
        /// <summary>
        /// Obtém um dependente a partir do ID da reserva
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<ReservaDependente> ObterPorIdReserva(int idReserva, bool habilitarTracking = false);

        /// <summary>
        /// Verifica se existe um dependente para a reserva
        /// </summary>
        Task<bool> VerificarExistenciaPorReserva(int idReserva);

        /// <summary>
        /// Insere um dependente de uma reserva
        /// </summary>
        Task Inserir(ReservaDependente dependente);

        /// <summary>
        /// Deleta um dependente de uma reserva
        /// </summary>
        void Deletar(ReservaDependente dependente);
    }
}
