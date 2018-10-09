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
        public ReservaController(CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
            : base(cookieHelper, restSharpHelper)
        {

        }

        [Authorize(Policy = "MotoristaOuProprietarioCarro")]
        [HttpGet]
        [Route("listar-por-viagem/{idViagem:int}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a lista de reservas da viagem.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarPorViagem(int idViagem)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("reservas/obter-por-viagem/" + idViagem, Method.GET);

            var saida = ReservasSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as reservas da viagem.", saida.Mensagens, TipoAcaoAoOcultarFeedback.Ocultar));

            if (saida.Retorno.Any())
                ViewData["DescricaoViagem"] = saida.Retorno.First().Viagem.Descricao;
            else
            {
                apiResponse = await _restSharpHelper.ChamarApi("viagens/obter-por-id/" + idViagem, Method.GET);

                var viagemSaida = ViagemSaida.Obter(apiResponse.Content);

                ViewData["DescricaoViagem"] = viagemSaida.Sucesso ? viagemSaida.Retorno.Descricao : string.Empty;
            }

            return View(saida.Retorno);
        }

        //[Authorize(Policy = TipoPerfil.Administrador)]
        //[HttpGet]
        //[Route("cadastrar-localidade")]
        //public IActionResult CadastrarReserva()
        //{
        //    return PartialView("Manter", null);
        //}

        //[Authorize(Policy = TipoPerfil.Administrador)]
        //[HttpPost]
        //[Route("cadastrar-localidade")]
        //public async Task<IActionResult> CadastrarReserva(CadastrarReservaEntrada entrada)
        //{
        //    if (entrada == null)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do localidade não foram preenchidas.", new[] { "Verifique se todas as informações do localidade foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

        //    var parametros = new Parameter[]
        //    {
        //        new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
        //    };

        //    var apiResponse = await _restSharpHelper.ChamarApi("localidades/cadastrar", Method.POST, parametros);

        //    var saida = ReservaSaida.Obter(apiResponse.Content);

        //    if (saida == null)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar a localidade.", new[] { "A API não retornou nenhuma resposta." }));

        //    return !saida.Sucesso
        //        ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar a localidade.", saida.Mensagens))
        //        : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        //}

        //[Authorize(Policy = TipoPerfil.Administrador)]
        //[HttpGet]
        //[Route("alterar-localidade/{id:int:min(1)}")]
        //public async Task<IActionResult> AlterarReserva(int id)
        //{
        //    var apiResponse = await _restSharpHelper.ChamarApi("localidades/obter-por-id/" + id, Method.GET);

        //    var saida = ReservaSaida.Obter(apiResponse.Content);

        //    if (saida == null)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações da localidade.", new[] { "A API não retornou nenhuma resposta." }));

        //    if (!saida.Sucesso)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações da localidade.", saida.Mensagens));

        //    return PartialView("Manter", saida.Retorno);
        //}

        //[Authorize(Policy = TipoPerfil.Administrador)]
        //[HttpPost]
        //[Route("alterar-localidade")]
        //public async Task<IActionResult> AlterarReserva(AlterarReservaEntrada entrada)
        //{
        //    if (entrada == null)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da localidade não foram preenchidas.", new[] { "Verifique se todas as informações da localidade foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

        //    var parametros = new Parameter[]
        //    {
        //        new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
        //    };

        //    var apiResponse = await _restSharpHelper.ChamarApi("localidades/alterar", Method.PUT, parametros);

        //    var saida = Saida.Obter(apiResponse.Content);

        //    if (saida == null)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar a localidade.", new[] { "A API não retornou nenhuma resposta." }));

        //    return !saida.Sucesso
        //        ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar a localidade.", saida.Mensagens))
        //        : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        //}

        //[Authorize(Policy = TipoPerfil.Administrador)]
        //[HttpPost]
        //[Route("excluir-localidade/{id:int}")]
        //public async Task<IActionResult> ExcluirReserva(int id)
        //{
        //    var apiResponse = await _restSharpHelper.ChamarApi("localidades/excluir/" + id, Method.DELETE);

        //    var saida = Saida.Obter(apiResponse.Content);

        //    if (saida == null)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir a localidade.", new[] { "A API não retornou nenhuma resposta." }));

        //    return !saida.Sucesso
        //        ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir a localidade.", saida.Mensagens))
        //        : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Reserva excluída com sucesso."));
        //}
    }
}