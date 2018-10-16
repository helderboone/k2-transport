using JNogueira.Infraestrutura.Utilzao;
using K2.Web.Filters;
using K2.Web.Helpers;
using K2.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Linq;
using System.Threading.Tasks;

namespace K2.Web.Controllers
{
    public class ReservaController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public ReservaController(DatatablesHelper datatablesHelper, CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
            : base(cookieHelper, restSharpHelper)
        {
            _datatablesHelper = datatablesHelper;
        }

        [Authorize(Policy = "MotoristaOuProprietarioCarro")]
        [HttpGet]
        [Route("reservas-por-viagem/{idViagem:int}")]
        public async Task<IActionResult> ReservasPorViagem(int idViagem)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("viagens/obter-por-id/" + idViagem, Method.GET);

            var saida = ViagemSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as reservas da viagem.", new[] { "A API não retornou nenhuma resposta." }));

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as reservas da viagem.", saida.Mensagens));

            return View(saida.Retorno);
        }

        [Authorize(Policy = "MotoristaOuProprietarioCarro")]
        [HttpPost]
        [Route("listar-reservas-por-viagem/{idViagem:int}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a lista de reservas da viagem.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarReservasPorViagem(int idViagem)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("reservas/obter-por-viagem/" + idViagem, Method.GET);

            var saida = ReservasSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new DatatablesResult(_datatablesHelper.Draw, 0, null);

            if (saida.Retorno.Any())
                ViewData["DescricaoViagem"] = saida.Retorno.First().Viagem.Descricao;
            else
            {
                apiResponse = await _restSharpHelper.ChamarApi("viagens/obter-por-id/" + idViagem, Method.GET);

                var viagemSaida = ViagemSaida.Obter(apiResponse.Content);

                ViewData["DescricaoViagem"] = viagemSaida.Sucesso
                    ? viagemSaida.Retorno.Descricao
                    : string.Empty;
            }

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno.Count(), saida.Retorno.ToList());
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("cadastrar-reserva/{idViagem:int}")]
        public IActionResult CadastrarReserva(int idViagem)
        {
            ViewData["IdViagem"] = idViagem;

            return PartialView("Manter", null);
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("cadastrar-reserva")]
        public async Task<IActionResult> CadastrarReserva(CadastrarReservaEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da reserva não foram preenchidas.", new[] { "Verifique se todas as informações da reserva foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("reservas/cadastrar", Method.POST, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar a reserva.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar a reserva.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("alterar-reserva/{idReserva:int}")]
        public IActionResult AlterarReserva(int idReserva)
        {
            var apiResponse = _restSharpHelper.ChamarApi("reservas/obter-por-id/" + idReserva, Method.GET).Result;

            var saida = ReservaSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as informações da reserva.", new[] { "A API não retornou nenhuma resposta." }));

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível obter as informações da reserva.", saida.Mensagens));

            var reserva = saida.Retorno;

            return PartialView("Manter", reserva);
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("alterar-reserva")]
        public async Task<IActionResult> AlterarReserva(AlterarReservaEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da reserva não foram preenchidas.", new[] { "Verifique se todas as informações da reserva foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("reservas/alterar", Method.PUT, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar a reserva.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar a reserva.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("cadastrar-reserva-dependente/{idReserva:int}")]
        public IActionResult CadastrarReservaDependente(int idReserva)
        {
            var apiResponse = _restSharpHelper.ChamarApi("reservas/obter-por-id/" + idReserva, Method.GET).Result;

            var saida = ReservaSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as informações da reserva.", new[] { "A API não retornou nenhuma resposta." }));

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível obter as informações da reserva.", saida.Mensagens));

            var reserva = saida.Retorno;

            ViewData["IdReserva"] = idReserva;
            ViewData["NomeCliente"] = reserva.Cliente.Nome;

            return PartialView("ManterDependente", null);
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("cadastrar-reserva-dependente")]
        public async Task<IActionResult> CadastrarReservaDependente(CadastrarReservaDependenteEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do dependente não foram preenchidas.", new[] { "Verifique se todas as informações do dependente foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("reservas/cadastrar-dependente", Method.POST, parametros);

            var saida = ReservaDependenteSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o dependente para a reserva.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar o dependente para a reserva.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("alterar-reserva-dependente/{idReserva:int:min(1)}")]
        public async Task<IActionResult> AlterarReservaDependente(int idReserva)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("reservas/obter-dependente/" + idReserva, Method.GET);

            var saida = ReservaDependenteSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações do dependente da reserva.", new[] { "A API não retornou nenhuma resposta." }));

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações do dependente da reserva.", saida.Mensagens));

            apiResponse = _restSharpHelper.ChamarApi("reservas/obter-por-id/" + idReserva, Method.GET).Result;

            var reservaSaida = ReservaSaida.Obter(apiResponse.Content);

            if (reservaSaida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as informações da reserva.", new[] { "A API não retornou nenhuma resposta." }));

            if (!reservaSaida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível obter as informações da reserva.", saida.Mensagens));

            ViewData["NomeCliente"] = reservaSaida.Retorno.Cliente.Nome;

            return PartialView("ManterDependente", saida.Retorno);
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("alterar-reserva-dependente")]
        public async Task<IActionResult> AlterarReservaDependente(AlterarReservaDependenteEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do dependente da reserva não foram preenchidas.", new[] { "Verifique se todas as informações do dependente foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("reservas/alterar-dependente", Method.PUT, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar o dependente da reserva.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar o dependente da reserva.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("excluir-reserva-dependente/{idReserva:int:min(1)}")]
        public async Task<IActionResult> ExcluirReservaDependente(int idReserva)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("reservas/excluir-dependente/" + idReserva, Method.DELETE);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir o dependente da reserva.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir o dependente da reserva.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Dependente da reserva excluído com sucesso."));
        }
    }
}