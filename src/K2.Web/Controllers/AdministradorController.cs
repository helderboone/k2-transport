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
    public class AdministradorController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public AdministradorController(DatatablesHelper datatablesHelper, CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
            : base(cookieHelper, restSharpHelper)
        {
            _datatablesHelper = datatablesHelper;
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [Route("administradores")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("listar-administradores")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de administradores cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarAdministradores(ProcurarUsuarioEntrada filtro)
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

            var apiResponse = await _restSharpHelper.ChamarApi("usuarios/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno.TotalRegistros, saida.ObterRegistros<AdministradorRegistro>());
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpGet]
        [Route("cadastrar-administrador")]
        public IActionResult CadastrarAdministrador()
        {
            return PartialView("Manter", null);
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("cadastrar-administrador")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar o novo administrador.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarAdministrador(CadastrarUsuarioEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do administrador não foram preenchidas.", new[] { "Verifique se todas as informações do administrador foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("usuarios/cadastrar", Method.POST, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar o administrador.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpGet]
        [Route("alterar-administrador/{id:int:min(1)}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações do administrador.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarAdministrador(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("usuarios/obter-por-id/" + id, Method.GET);

            var saida = AdministradorSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações do administrador.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("alterar-administrador")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações do administrador.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarAdministrador(AlterarUsuarioEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do administrador não foram preenchidas.", new[] { "Verifique se todas as informações do administrador foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("usuarios/alterar", Method.PUT, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar o administrador.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("excluir-administrador/{id:int}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir o administrador.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirAdministrador(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("usuarios/excluir/" + id, Method.DELETE);

            var saida = Saida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir o administrador.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Administrador excluído com sucesso."));
        }
    }
}