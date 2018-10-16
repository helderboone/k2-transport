using JNogueira.Infraestrutura.Utilzao;
using K2.Web.Filters;
using K2.Web.Helpers;
using K2.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace K2.Web.Controllers
{
    public class ViagemController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public ViagemController(DatatablesHelper datatablesHelper, CookieHelper cookieHelper, RestSharpHelper restSharpHelper)
            : base(cookieHelper, restSharpHelper)
        {
            _datatablesHelper = datatablesHelper;
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [Route("viagens")]
        public IActionResult Index()
        {
            return View();
        }

        //[Authorize(Policy = "MotoristaOuProprietarioCarro")]
        //[HttpGet]
        //[Route("viagens-previstas")]
        //public async Task<IActionResult> ViagensPrevistas()
        //{
        //    var filtro = new ProcurarViagemEntrada
        //    {
        //        OrdenarPor = "DataHorarioSaida",
        //        SomentePrevistas = true
        //    };

        //    var parametros = new Parameter[]
        //    {
        //        new Parameter{ Name = "filtro", Value = filtro.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
        //    };

        //    var apiResponse = await _restSharpHelper.ChamarApi("viagens/procurar", Method.POST, parametros);

        //    var saida = ProcurarSaida.Obter(apiResponse.Content);

        //    if (!saida.Sucesso)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as viagens previstas.", saida.Mensagens, TipoAcaoAoOcultarFeedback.Ocultar));

        //    var registros = saida.ObterRegistros<ViagemRegistro>();

        //    return View(registros);
        //}

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("listar-viagens-previstas")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a lista de viagens previstas.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarViagensPrevistas()
        {
            var filtro = new ProcurarViagemEntrada
            {
                OrdenarPor = "DataHorarioSaida",
                SomentePrevistas = true
            };

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "filtro", Value = filtro.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("viagens/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno.TotalRegistros, saida.ObterRegistros<ViagemRegistro>().ToList());
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("cadastrar-viagem")]
        public IActionResult CadastrarViagem()
        {
            return PartialView("Manter", null);
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("cadastrar-viagem")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar a viagem.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarViagem(CadastrarViagemEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do viagem não foram preenchidas.", new[] { "Verifique se todas as informações do viagem foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("viagens/cadastrar", Method.POST, parametros);

            var saida = ViagemSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar a viagem.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar a viagem.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("alterar-viagem/{id:int:min(1)}")]
        public async Task<IActionResult> AlterarViagem(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("viagens/obter-por-id/" + id, Method.GET);

            var saida = ViagemSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações da viagem.", new[] { "A API não retornou nenhuma resposta." }));

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações da viagem.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("alterar-viagem")]
        public async Task<IActionResult> AlterarViagem(AlterarViagemEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da viagem não foram preenchidas.", new[] { "Verifique se todas as informações da viagem foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await _restSharpHelper.ChamarApi("viagens/alterar", Method.PUT, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar a viagem.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar a viagem.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("excluir-viagem/{id:int}")]
        public async Task<IActionResult> ExcluirViagem(int id)
        {
            var apiResponse = await _restSharpHelper.ChamarApi("viagens/excluir/" + id, Method.DELETE);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir a viagem.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir a viagem.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Viagem excluída com sucesso."));
        }
    }
}