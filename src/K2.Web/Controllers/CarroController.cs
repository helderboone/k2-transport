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

        [Authorize(Policy = TipoPerfil.ProprietarioCarro)]
        [Route("carros")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = TipoPerfil.ProprietarioCarro)]
        [HttpPost]
        [Route("listar-carros")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a lista de carros cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
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

            var apiResponse = await base.ChamarApi("carros/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            return new DatatablesResult(_datatablesHelper.Draw, saida);
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("cadastrar-carro")]
        public IActionResult CadastrarCarro()
        {
            return PartialView("Manter", null);
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("cadastrar-carro")]
        public async Task<IActionResult> CadastrarCarro(CadastrarCarroEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do carro não foram preenchidas.", new[] { "Verifique se todas as informações do carro foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await base.ChamarApi("carros/cadastrar", Method.POST, parametros);

            var saida = CarroSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o carro.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar o carro.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("alterar-carro/{id:int:min(1)}")]
        public async Task<IActionResult> AlterarCarro(int id)
        {
            var apiResponse = await base.ChamarApi("carros/obter-por-id/" + id, Method.GET);

            var saida = CarroSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações do carro.", new[] { "A API não retornou nenhuma resposta." }));

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações do carro.", saida.Mensagens));

            return PartialView("Manter", saida.ObterRetorno());
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("alterar-carro")]
        public async Task<IActionResult> AlterarCarro(AlterarCarroEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do carro não foram preenchidas.", new[] { "Verifique se todas as informações da carro foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await base.ChamarApi("carros/alterar", Method.PUT, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar o carro.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar o carro.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("excluir-carro/{id:int}")]
        public async Task<IActionResult> ExcluirCarro(int id)
        {
            var apiResponse = await base.ChamarApi("carros/excluir/" + id, Method.DELETE);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir o carro.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir o carro.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Carro excluído com sucesso."));
        }
    }
}