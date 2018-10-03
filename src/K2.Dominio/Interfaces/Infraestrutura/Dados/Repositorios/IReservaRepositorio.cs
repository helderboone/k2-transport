using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios
{
    public interface IReservaRepositorio
    {
        /// <summary>
        /// Obtém uma reserva a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Reserva> ObterPorId(int id, bool habilitarTracking = false);

        /// <summary>
        /// Obtém todas as reservas relacionadas a uma viagem a partir do ID da viagem
        /// </summary>
        Task<IEnumerable<Reserva>> ObterPorIdVigem(int idViagem);

        /// <summary>
        /// Verifica se existe uma reserva a partir do seu ID.
        /// </summary>
        Task<bool> VerificarExistenciaPorId(int id);

        /// <summary>
        /// Verifica se existe uma reserva na viagem para o mesmo cliente
        /// </summary>
        Task<bool> VerificarExistenciaPorClienteViagem(int idCliente, int idViagem, int? idReserva = null);

        /// <summary>
        /// Obtém as reservas baseadas nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarReservaEntrada entrada, CredencialUsuarioEntrada credencial);

        /// <summary>
        /// Insere uma nova reserva
        /// </summary>
        Task Inserir(Reserva reserva);

        /// <summary>
        /// Deleta uma reserva
        /// </summary>
        void Deletar(Reserva reserva);
    }
}
