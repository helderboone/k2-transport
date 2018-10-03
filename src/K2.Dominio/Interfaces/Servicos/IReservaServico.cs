using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Reserva"
    /// </summary>
    public interface IReservaServico
    {
        /// <summary>
        /// Obtém uma reserva a partir do seu ID
        /// </summary>
        Task<ISaida> ObterReservaPorId(int id, CredencialUsuarioEntrada credencial);

        /// <summary>
        /// Obtém todas as reservas relacionadas a uma viagem a partir do ID da viagem
        /// </summary>
        Task<ISaida> ObterReservasPorViagem(int idViagem, CredencialUsuarioEntrada credencial);

        /// <summary>
        /// Obtém as reservas baseadas nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarReservas(ProcurarReservaEntrada entrada, CredencialUsuarioEntrada credencial);

        /// <summary>
        /// Realiza o cadastro de uma nova reserva.
        /// </summary>
        Task<ISaida> CadastrarReserva(CadastrarReservaEntrada entrada);

        /// <summary>
        /// Realiza a alteração de uma reserva.
        /// </summary>
        Task<ISaida> AlterarReserva(AlterarReservaEntrada entrada);

        /// <summary>
        /// Exclui uma reserva a partir do seu ID
        /// </summary>
        Task<ISaida> ExcluirReserva(int id);

        /// <summary>
        /// Obtém o dependente de uma reserva
        /// </summary>
        Task<ISaida> ObterDependentePorReserva(int idReserva);

        /// <summary>
        /// Realiza o cadastro de um dependente para uma determinada reserva.
        /// </summary>
        Task<ISaida> CadastrarDependente(CadastrarReservaDependenteEntrada entrada);

        /// <summary>
        /// Realiza a alteração do dependente de uma reserva.
        /// </summary>
        Task<ISaida> AlterarDependente(AlterarReservaDependenteEntrada entrada);

        /// <summary>
        /// Exclui o dependente de uma reserva
        /// </summary>
        Task<ISaida> ExcluirDependente(int idReserva);
    }
}
