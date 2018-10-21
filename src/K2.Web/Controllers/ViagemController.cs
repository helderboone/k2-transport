using K2.Infraestrutura;
using K2.Web.Filters;
using K2.Web.Helpers;
using K2.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using Rotativa.AspNetCore;
using System.Linq;
using System.Threading.Tasks;

namespace K2.Web.Controllers
{
    public class ViagemController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;
        private readonly ILogger _logger;

        public ViagemController(DatatablesHelper datatablesHelper, CookieHelper cookieHelper, RestSharpHelper restSharpHelper, ILogger<ViagemController> logger)
            : base(cookieHelper, restSharpHelper)
        {
            _datatablesHelper = datatablesHelper;
            _logger = logger;
        }

        [Route("viagens")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("obter-info-viagem/{id:int}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações da viagem.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ObterInformacoesViagem(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("viagens/obter-por-id/" + id, Method.GET);

            var saida = ViagemSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações da viagem.", saida.Mensagens));

            return PartialView("Informacoes", saida.Retorno);
        }

        [HttpPost]
        [Route("listar-viagens-previstas")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de viagens previstas.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarViagensPrevistas()
        {
            var filtro = new ProcurarViagemEntrada
            {
                OrdenarPor = "DataHorarioSaida",
                SomentePrevistas = true
            };

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "filtro", Value = filtro.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("viagens/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Ocorreu um erro ao obter a relação de viagens previstas.", saida.Mensagens));

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno.TotalRegistros, saida.ObterRegistros<ViagemRegistro>().ToList());
        }

        [HttpPost]
        [Route("listar-viagens-realizadas")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de viagens realizadas.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarViagensRealizadas(ProcurarViagemEntrada filtro)
        {
            filtro.OrdenarPor = _datatablesHelper.OrdenarPor;
            filtro.OrdenarSentido = _datatablesHelper.OrdenarSentido;
            filtro.PaginaIndex = _datatablesHelper.PaginaIndex;
            filtro.PaginaTamanho = _datatablesHelper.PaginaTamanho;

            filtro.SomenteRealizadasOuCanceladas = true;

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "filtro", Value = filtro.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("viagens/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Ocorreu um erro ao obter a relação de viagens realizadas.", saida.Mensagens));

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno.TotalRegistros, saida.ObterRegistros<ViagemRegistro>().ToList());
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("cadastrar-viagem")]
        public IActionResult CadastrarViagem()
        {
            return PartialView("Manter", null);
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("cadastrar-viagem")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar a nova viagem.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarViagem(CadastrarViagemEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do viagem não foram preenchidas.", new[] { "Verifique se todas as informações do viagem foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("viagens/cadastrar", Method.POST, parametros);

            var saida = ViagemSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar a viagem.", saida.Mensagens));

            _logger.LogInformation($"Nova viagem cadastrada: {saida.Retorno.Descricao}");

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("alterar-viagem/{id:int:min(1)}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações da viagem.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarViagem(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("viagens/obter-por-id/" + id, Method.GET);

            var saida = ViagemSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações da viagem.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("alterar-viagem")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações da viagem.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarViagem(AlterarViagemEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da viagem não foram preenchidas.", new[] { "Verifique se todas as informações da viagem foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("viagens/alterar", Method.PUT, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar a viagem.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("excluir-viagem/{id:int}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir a viagem.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirViagem(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("viagens/excluir/" + id, Method.DELETE);

            var saida = Saida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir a viagem.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Viagem excluída com sucesso."));
        }

        [HttpGet]
        [Route("gerar-pdf-demonstrativo/{idViagem:int}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao gerar o demonstrativo da viagem.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> GerarDemonstrativoPdf(int idViagem, int tipo)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("viagens/obter-por-id/" + idViagem, Method.GET);

            var saida = ViagemSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações da viagem.", saida.Mensagens));

            _logger.LogInformation($"Demonstrativo da viagem \"{saida.Retorno.Descricao}\" gerado.");

            var footer = "--footer-right \"Gerado em: " + DateTimeHelper.ObterHorarioAtualBrasilia().ToString("dd/MM/yyyy HH:mm") + "\" " + "--footer-left \"Página: [page] de [toPage]\" --footer-line --footer-font-size \"8\" --footer-spacing 1 --footer-font-name \"Roboto\"";

            return new ViewAsPdf("Demonstrativo", saida.Retorno) { CustomSwitches = footer };
        }
    }
}