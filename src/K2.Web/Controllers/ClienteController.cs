using K2.Api.Models;
using K2.Web.Filters;
using K2.Web.Helpers;
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
        [HttpPost]
        [Route("listar-clientes")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as lista clientes cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarClientes(ProcurarClienteEntrada filtro)
        {
            if (filtro == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações para a procura não foram preenchidas.", tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));

            var dataTablesParams = new DatatablesHelper(_httpContextAccessor);

            filtro.PaginaIndex   = dataTablesParams.PaginaIndex;
            filtro.PaginaTamanho = dataTablesParams.PaginaTamanho;

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "filtro", Value = filtro.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await base.ChamarApi("clientes/procurar", Method.POST, parametros);

            var saida = Saida.Obter(apiResponse.Content);


            return new EmptyResult();
            
            //try
            //{
            //    filtro.PaginaIndex = parametros.PaginaIndex;
            //    filtro.PaginaTamanho = parametros.PaginaTamanho;
            //    filtro.TipoPerfil = new[] { TipoPerfilUsuario.Cliente };

            //    switch (parametros.OrdenarPor)
            //    {
            //        case "Nome":
            //            filtro.ModificarOrdenacao(x => x.Nome, parametros.OrdenarSentido);
            //            break;
            //        case "Email":
            //            filtro.ModificarOrdenacao(x => x.Email, parametros.OrdenarSentido);
            //            break;
            //        case "FlagAtivo":
            //            filtro.ModificarOrdenacao(x => x.FlagAtivo.ToString(), parametros.OrdenarSentido);
            //            break;
            //    }

            //    var lst =
            //        _usuarioService.Procurar(filtro)
            //            .Select(
            //                x =>
            //                    new
            //                    {
            //                        x.IdUsuario,
            //                        x.Nome,
            //                        x.Email,
            //                        x.FlagAtivo,
            //                        x.Telefone,
            //                        Condominios = x.Condominios.Any()
            //                            ? string.Join("<br/>", x.Condominios.OrderBy(y => y.Nome).Select(y => y.Nome.ToUpper()))
            //                            : string.Empty
            //                    })
            //            .ToList();

            //    return base.ObterDataSourceDataTables(parametros.Draw, lst, filtro.TotalRegistros);
            //}
            //catch (Exception ex)
            //{
            //    return RetornarJsonMensagemPorException(ex, TipoAcaoOcultarMensagem.OcultarPopups, "Ocorreu um erro ao carregar a listagem de clientes.");
            //}
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
        public async Task<IActionResult> CadastrarCliente(CadastrarClienteEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do cliente não foram preenchidas.", new[] { "Verifique se todas as informações do cliente foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "cadastrarClienteEntrada", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
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