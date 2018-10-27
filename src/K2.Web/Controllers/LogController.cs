using K2.Web.Filters;
using K2.Web.Helpers;
using K2.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Threading.Tasks;

namespace K2.Web.Controllers
{
    public class LogController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public LogController(DatatablesHelper datatablesHelper, CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
            : base(cookieHelper, restSharpHelper)
        {
            _datatablesHelper = datatablesHelper;
        }

        [Authorize(Policy = TipoPoliticaAcesso.AnalistaTI)]
        [Route("log")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = TipoPoliticaAcesso.AnalistaTI)]
        [HttpPost]
        [Route("listar-log")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter o log.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarLog(ProcurarLogEntrada filtro)
        {
            if (filtro == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações para a procura não foram preenchidas.", tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));

            filtro.OrdenarPor     = _datatablesHelper.OrdenarPor == "dataToString" ? "data" : _datatablesHelper.OrdenarPor;
            filtro.OrdenarSentido = _datatablesHelper.OrdenarSentido;
            filtro.PaginaIndex    = _datatablesHelper.PaginaIndex;
            filtro.PaginaTamanho  = _datatablesHelper.PaginaTamanho;

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "filtro", Value = filtro.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("log/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno.TotalRegistros, saida.ObterRegistros<LogRegistro>());
        }

        [Authorize(Policy = TipoPoliticaAcesso.AnalistaTI)]
        [HttpGet]
        [Route("detalhar-log/{id:int:min(1)}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações do registro do log.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> DetalharLog(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("log/obter-por-id/" + id, Method.GET);

            var saida = LogSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações do registro do log.", saida.Mensagens));

            return PartialView("Detalhar", saida.Retorno);
        }
    }
}