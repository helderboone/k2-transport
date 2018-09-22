using K2.Web.Filters;
using K2.Web.Helpers;
using K2.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Linq;
using System.Threading.Tasks;

namespace K2.Web.Controllers
{
    public class ProprietarioCarroController : BaseController
    {
        public ProprietarioCarroController(IConfiguration configuration, ILogger<ProprietarioCarroController> logger, IHttpContextAccessor httpContextAccessor)
            : base(configuration, logger, httpContextAccessor)
        {

        }

        [Authorize(Policy = "Administrador")]
        [Route("proprietarios")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("listar-proprietarios")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as lista proprietários cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarProprietarioCarros(ProcurarProprietarioCarroEntrada filtro)
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

            var apiResponse = await base.ChamarApi("proprietarios-carro/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            return new DatatablesResult(dataTablesParams.Draw, saida);
        }

        [Authorize(Policy = "Administrador")]
        [HttpGet]
        [Route("cadastrar-proprietario")]
        public IActionResult CadastrarProprietarioCarro()
        {
            return PartialView("Manter", null);
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("cadastrar-proprietario")]
        public async Task<IActionResult> CadastrarProprietarioCarro(CadastrarProprietarioCarroEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do proprietário não foram preenchidas.", new[] { "Verifique se todas as informações do proprietário foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await base.ChamarApi("proprietarios-carro/cadastrar", Method.POST, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o proprietário.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar o proprietário.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = "Administrador")]
        [HttpGet]
        [Route("alterar-proprietario/{id:int:min(1)}")]
        public async Task<IActionResult> AlterarProprietarioCarro(int id)
        {
            var apiResponse = await base.ChamarApi("proprietarios-carro/obter-por-id/" + id, Method.GET);

            var saida = ProprietarioCarroSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações do proprietário.", new[] { "A API não retornou nenhuma resposta." }));

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações do proprietário.", saida.Mensagens));

            return PartialView("Manter", saida.ObterRetorno());
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("alterar-proprietario")]
        public async Task<IActionResult> AlterarProprietarioCarro(AlterarProprietarioCarroEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do proprietário não foram preenchidas.", new[] { "Verifique se todas as informações do proprietario foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await base.ChamarApi("proprietarios-carro/alterar", Method.PUT, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar o proprietário.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar o proprietário.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("excluir-proprietario/{id:int}")]
        public async Task<IActionResult> ExcluirProprietarioCarro(int id)
        {
            var apiResponse = await base.ChamarApi("proprietarios-carro/excluir/" + id, Method.DELETE);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir o proprietário.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir o proprietário.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Proprietário excluído com sucesso."));
        }
    }
}