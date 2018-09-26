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
    public class MotoristaController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public MotoristaController(DatatablesHelper datatablesHelper, CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
            : base(cookieHelper, restSharpHelper)
        {
            _datatablesHelper = datatablesHelper;
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [Route("motoristas")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("listar-motoristas")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as lista motoristas cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarMotoristas(ProcurarMotoristaEntrada filtro)
        {
            if (filtro == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações para a procura não foram preenchidas.", tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));

            var dataTablesParams = new DatatablesHelper(_httpContextAccessor);

            filtro.OrdenarPor     = dataTablesParams.OrdenarPor;
            filtro.OrdenarSentido = dataTablesParams.OrdenarSentido;
            filtro.PaginaIndex    = dataTablesParams.PaginaIndex;
            filtro.PaginaTamanho  = dataTablesParams.PaginaTamanho;

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "filtro", Value = filtro.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await base.ChamarApi("motoristas/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            return new DatatablesResult(dataTablesParams.Draw, saida);
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("cadastrar-motorista")]
        public IActionResult CadastrarMotorista()
        {
            return PartialView("Manter", null);
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("cadastrar-motorista")]
        public async Task<IActionResult> CadastrarMotorista(CadastrarMotoristaEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do motorista não foram preenchidas.", new[] { "Verifique se todas as informações do motorista foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await base.ChamarApi("motoristas/cadastrar", Method.POST, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o motorista.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar o motorista.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("alterar-motorista/{id:int:min(1)}")]
        public async Task<IActionResult> AlterarMotorista(int id)
        {
            var apiResponse = await base.ChamarApi("motoristas/obter-por-id/" + id, Method.GET);

            var saida = MotoristaSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações do motorista.", new[] { "A API não retornou nenhuma resposta." }));

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações do motorista.", saida.Mensagens));

            return PartialView("Manter", saida.ObterRetorno());
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("alterar-motorista")]
        public async Task<IActionResult> AlterarMotorista(AlterarMotoristaEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do motorista não foram preenchidas.", new[] { "Verifique se todas as informações do motorista foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await base.ChamarApi("motoristas/alterar", Method.PUT, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar o motorista.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar o motorista.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("excluir-motorista/{id:int}")]
        public async Task<IActionResult> ExcluirMotorista(int id)
        {
            var apiResponse = await base.ChamarApi("motoristas/excluir/" + id, Method.DELETE);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir o motorista.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir o motorista.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Motorista excluído com sucesso."));
        }
    }
}