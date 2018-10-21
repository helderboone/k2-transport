using K2.Dominio;
using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace K2.Api.Controllers
{
    [Produces("application/json")]
    public class ReservaController : BaseController
    {
        private readonly IReservaServico _reservaServico;

        public ReservaController(IReservaServico reservaServico)
        {
            _reservaServico = reservaServico;
        }

        /// <summary>
        /// Obtém uma reserva a partir do seu ID
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.MotoristaOuProprietarioCarro)]
        [HttpGet]
        [Route("v1/reservas/obter-por-id/{id:int}")]
        public async Task<ISaida> ObterPorId(int id)
        {
            var credencial = new CredencialUsuarioEntrada(base.ObterIdUsuario(), base.ObterPerfilUsuario());

            return await _reservaServico.ObterReservaPorId(id, credencial);
        }

        /// <summary>
        /// Obtém as reservas relacionadas a um viagem a partir do ID da viagem
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.MotoristaOuProprietarioCarro)]
        [HttpGet]
        [Route("v1/reservas/obter-por-viagem/{idViagem:int}")]
        public async Task<ISaida> ObterPorViagem(int idViagem)
        {
            var credencial = new CredencialUsuarioEntrada(base.ObterIdUsuario(), base.ObterPerfilUsuario());

            return await _reservaServico.ObterReservasPorViagem(idViagem, credencial);
        }

        /// <summary>
        /// Realiza uma procura por reservas a partir dos parâmetros informados
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.MotoristaOuProprietarioCarro)]
        [HttpPost]
        [Route("v1/reservas/procurar")]
        public async Task<ISaida> Procurar([FromBody] ProcurarReservaEntrada entrada)
        {
            var credencial = new CredencialUsuarioEntrada(base.ObterIdUsuario(), base.ObterPerfilUsuario());

            return await _reservaServico.ProcurarReservas(entrada, credencial);
        }

        /// <summary>
        /// Realiza o cadastro de uma nova reserva
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("v1/reservas/cadastrar")]
        public async Task<ISaida> Cadastrar([FromBody] CadastrarReservaEntrada entrada)
        {
            return await _reservaServico.CadastrarReserva(entrada);
        }

        /// <summary>
        /// Realiza a alteração de uma reserva
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPut]
        [Route("v1/reservas/alterar")]
        public async Task<ISaida> Alterar([FromBody] AlterarReservaEntrada entrada)
        {
            return await _reservaServico.AlterarReserva(entrada);
        }

        /// <summary>
        /// Realiza a exclusão de um reserva.
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpDelete]
        [Route("v1/reservas/excluir/{id:int}")]
        public async Task<ISaida> ExcluirReserva(int id)
        {
            return await _reservaServico.ExcluirReserva(id);
        }

        /// <summary>
        /// Obtém o dependente de uma reserva
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.MotoristaOuProprietarioCarro)]
        [HttpGet]
        [Route("v1/reservas/obter-dependente/{idReserva:int}")]
        public async Task<ISaida> ObterDependentePorReserva(int idReserva)
        {
            return await _reservaServico.ObterDependentePorReserva(idReserva);
        }

        /// <summary>
        /// Realiza o cadastro do dependente da reserva
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("v1/reservas/cadastrar-dependente")]
        public async Task<ISaida> CadastrarDependente([FromBody] CadastrarReservaDependenteEntrada entrada)
        {
            return await _reservaServico.CadastrarDependente(entrada);
        }

        /// <summary>
        /// Realiza a alteração do dependente da reserva
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPut]
        [Route("v1/reservas/alterar-dependente")]
        public async Task<ISaida> AlterarDependente([FromBody] AlterarReservaDependenteEntrada entrada)
        {
            return await _reservaServico.AlterarDependente(entrada);
        }

        /// <summary>
        /// Realiza a exclusão de um dependente.
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpDelete]
        [Route("v1/reservas/excluir-dependente/{idReserva:int}")]
        public async Task<ISaida> ExcluirDependente(int idReserva)
        {
            return await _reservaServico.ExcluirDependente(idReserva);
        }
    }
}