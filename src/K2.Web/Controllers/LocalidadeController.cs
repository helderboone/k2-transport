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
    public class LocalidadeController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public LocalidadeController(DatatablesHelper datatablesHelper, CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
            : base(cookieHelper, restSharpHelper)
        {
            _datatablesHelper = datatablesHelper;
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [Route("localidades")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("listar-localidades")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de localidades cadastradas.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarLocalidades(ProcurarLocalidadeEntrada filtro)
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

            var apiResponse = await _restSharpHelper.ChamarApi("localidades/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno.TotalRegistros, saida.ObterRegistros<LocalidadeRegistro>());
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpGet]
        [Route("cadastrar-localidade")]
        public IActionResult CadastrarLocalidade()
        {
            return PartialView("Manter", null);
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("cadastrar-localidade")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar a nova localidade.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarLocalidade(CadastrarLocalidadeEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do localidade não foram preenchidas.", new[] { "Verifique se todas as informações do localidade foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("localidades/cadastrar", Method.POST, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar a localidade.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpGet]
        [Route("alterar-localidade/{id:int:min(1)}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações da localidade.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarLocalidade(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("localidades/obter-por-id/" + id, Method.GET);

            var saida = LocalidadeSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações da localidade.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("alterar-localidade")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações da localidade.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarLocalidade(AlterarLocalidadeEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da localidade não foram preenchidas.", new[] { "Verifique se todas as informações da localidade foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("localidades/alterar", Method.PUT, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar a localidade.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("excluir-localidade/{id:int}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir a localidade.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirLocalidade(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("localidades/excluir/" + id, Method.DELETE);

            var saida = Saida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir a localidade.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Localidade excluída com sucesso."));
        }
    }
}