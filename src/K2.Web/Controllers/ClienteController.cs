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
    public class ClienteController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public ClienteController(DatatablesHelper datatablesHelper, CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
            : base(cookieHelper, restSharpHelper)
        {
            _datatablesHelper = datatablesHelper;
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [Route("clientes")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("listar-clientes")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de clientes cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarClientes(ProcurarClienteEntrada filtro)
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

            var apiResponse = await _restSharpHelper.ChamarApi("clientes/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno.TotalRegistros, saida.ObterRegistros<ClienteRegistro>());
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpGet]
        [Route("obter-clientes-por-palavra-chave")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de clientes cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ObterClientesPorTermo(string palavraChave)
        {
            var filtro = new ProcurarClienteEntrada
            {
                PalavraChave = palavraChave,
                OrdenarPor = "Nome",
                OrdenarSentido = "ASC",
                PaginaIndex = 1,
                PaginaTamanho = 30
            };

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "filtro", Value = filtro.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("clientes/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            return new JsonResult(saida.ObterRegistros<ClienteRegistro>());
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpGet]
        [Route("cadastrar-cliente")]
        public IActionResult CadastrarCliente()
        {
            return PartialView("Manter", null);
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("cadastrar-cliente")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar o novo cliente.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarCliente(CadastrarClienteEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do cliente não foram preenchidas.", new[] { "Verifique se todas as informações do cliente foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("clientes/cadastrar", Method.POST, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar o cliente.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpGet]
        [Route("alterar-cliente/{id:int:min(1)}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações do cliente.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarCliente(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("clientes/obter-por-id/" + id, Method.GET);

            var saida = ClienteSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações do cliente.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("alterar-cliente")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações do cliente.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarCliente(AlterarClienteEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do cliente não foram preenchidas.", new[] { "Verifique se todas as informações do cliente foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("clientes/alterar", Method.PUT, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar o cliente.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("excluir-cliente/{id:int}")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir o cliente.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirCliente(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("clientes/excluir/" + id, Method.DELETE);

            var saida = Saida.Obter(apiResponse.Content);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir o cliente.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Cliente excluído com sucesso."));
        }
    }
}