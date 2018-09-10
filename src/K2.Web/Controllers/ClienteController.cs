using K2.Api.ViewModels;
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
    public class ClienteController : BaseController
    {
        public ClienteController(IConfiguration configuration, ILogger<ClienteController> logger, IHttpContextAccessor httpContextAccessor)
            : base(configuration, logger, httpContextAccessor)
        {

        }

        [Authorize(Policy = "Administrador")]
        [Route("clientes")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Administrador")]
        [HttpGet]
        [Route("cadastrar-cliente")]
        public IActionResult CadastrarCliente()
        {
            return PartialView("Manter", null);
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("cadastrar-cliente")]
        public async Task<IActionResult> CadastrarCliente(CadastrarClienteViewModel cadastrarClienteEntrada)
        {
            if (cadastrarClienteEntrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do cliente não foram preenchidas.", new[] { "Verifique se todas as informações do cliente foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "cadastrarClienteEntrada", Value = cadastrarClienteEntrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await base.ChamarApi("clientes/cadastrar", Method.POST, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o cliente.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar o cliente.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }
    }
}