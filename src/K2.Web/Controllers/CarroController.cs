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
    public class CarroController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public CarroController(DatatablesHelper datatablesHelper, CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
            : base(cookieHelper, restSharpHelper)
        {
            _datatablesHelper = datatablesHelper;
        }

        [Authorize(Policy = TipoPoliticaAcesso.ProprietarioCarro)]
        [Route("carros")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = TipoPoliticaAcesso.ProprietarioCarro)]
        [HttpPost]
        [Route("listar-carros")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de carros cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarCarros(ProcurarCarroEntrada filtro)
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

            var apiResponse = await _restSharpHelper.ChamarApi("carros/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno.TotalRegistros, saida.ObterRegistros<CarroRegistro>());
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpGet]
        [Route("cadastrar-carro")]
        public IActionResult CadastrarCarro()
        {
            return PartialView("Manter", null);
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("cadastrar-carro")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar o novo carro.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarCarro(CadastrarCarroEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do carro não foram preenchidas.", new[] { "Verifique se todas as informações do carro foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("carros/cadastrar", Method.POST, parametros);

            var saida = CarroSaida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar o carro.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpGet]
        [Route("alterar-carro/{id:int:min(1)}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações do carro.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarCarro(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("carros/obter-por-id/" + id, Method.GET);

            var saida = CarroSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações do carro.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("alterar-carro")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações do carro.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarCarro(AlterarCarroEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do carro não foram preenchidas.", new[] { "Verifique se todas as informações da carro foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("carros/alterar", Method.PUT, parametros);

            var saida = CarroSaida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar o carro.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("excluir-carro/{id:int}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir o carro.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirCarro(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("carros/excluir/" + id, Method.DELETE);

            var saida = CarroSaida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir o carro.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Carro excluído com sucesso."));
        }

        [Authorize(Policy = TipoPoliticaAcesso.ProprietarioCarro)]
        [HttpGet]
        [Route("visualizar-carro/{id:int}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações do carro.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> VisualizarCarro(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("carros/obter-por-id/" + id, Method.GET);

            var saida = CarroSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações do carro.", saida.Mensagens));

            return PartialView("Visualizar", saida.Retorno);
        }
    }
}