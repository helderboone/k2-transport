using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using System;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios
{
    public interface IViagemRepositorio
    {
        /// <summary>
        /// Obtém uma viagem a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Viagem> ObterPorId(int id, bool habilitarTracking = false);

        /// <summary>
        /// Verifica se existe uma viagem com o motorista e data de saída informados
        /// </summary>
        Task<bool> VerificarExistenciaPorMotoristaDataHorarioSaida(int idMotorista, DateTime dataHorarioSaida, int? idViagem = null);

        /// <summary>
        /// Verifica se existe uma viagem com o motorista e data de saída informados
        /// </summary>
        Task<bool> VerificarExistenciaPorCarroDataHorarioSaida(int idCarro, DateTime dataHorarioSaida, int? idViagem = null);

        /// <summary>
        /// Obtém as viagems baseadas nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarViagemEntrada entrada, CredencialUsuarioEntrada credencial);

        /// <summary>
        /// Insere uma nova viagem
        /// </summary>
        Task Inserir(Viagem viagem);

        /// <summary>
        /// Deleta uma viagem
        /// </summary>
        void Deletar(Viagem viagem);
    }
}
