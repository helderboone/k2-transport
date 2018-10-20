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
    public class ProprietarioCarroController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public ProprietarioCarroController(DatatablesHelper datatablesHelper, CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
            : base(cookieHelper, restSharpHelper)
        {
            _datatablesHelper = datatablesHelper;
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [Route("proprietarios")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("listar-proprietarios")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de proprietários cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarProprietarioCarros(ProcurarProprietarioCarroEntrada filtro)
        {
            if (filtro == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações para a procura não foram preenchidas.", tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));

            filtro.OrdenarPor     = _datatablesHelper.OrdenarPor;
            filtro.OrdenarSentido = _datatablesHelper.OrdenarSentido;
            filtro.PaginaIndex    = _datatablesHelper.PaginaIndex;
            filtro.PaginaTamanho  = _datatablesHelper.PaginaTamanho;

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "filtro", Value = filtro.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("proprietarios-carro/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno.TotalRegistros, saida.ObterRegistros<ProprietarioCarroRegistro>());
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpGet]
        [Route("cadastrar-proprietario")]
        public IActionResult CadastrarProprietarioCarro()
        {
            return PartialView("Manter", null);
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("cadastrar-proprietario")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar o novo proprietário.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarProprietarioCarro(CadastrarProprietarioCarroEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do proprietário não foram preenchidas.", new[] { "Verifique se todas as informações do proprietário foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("proprietarios-carro/cadastrar", Method.POST, parametros);

            var saida = ProprietarioCarroSaida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar o proprietário.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpGet]
        [Route("alterar-proprietario/{id:int:min(1)}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações do proprietário.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarProprietarioCarro(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("proprietarios-carro/obter-por-id/" + id, Method.GET);

            var saida = ProprietarioCarroSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações do proprietário.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("alterar-proprietario")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações do proprietário.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarProprietarioCarro(AlterarProprietarioCarroEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do proprietário não foram preenchidas.", new[] { "Verifique se todas as informações do proprietário foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("proprietarios-carro/alterar", Method.PUT, parametros);

            var saida = ProprietarioCarroSaida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar o proprietário.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("excluir-proprietario/{id:int}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir o proprietário.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirProprietarioCarro(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("proprietarios-carro/excluir/" + id, Method.DELETE);

            var saida = ProprietarioCarroSaida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir o proprietário.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Proprietário excluído com sucesso."));
        }

        [Authorize(Policy = TipoPoliticaAcesso.ProprietarioCarro)]
        [HttpGet]
        [Route("visualizar-proprietario/{id:int}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações do proprietário.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> VisualizarProprietarioCarro(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("proprietarios-carro/obter-por-id/" + id, Method.GET);

            var saida = ProprietarioCarroSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações do proprietário.", saida.Mensagens));

            return PartialView("Visualizar", saida.Retorno);
        }
    }
}