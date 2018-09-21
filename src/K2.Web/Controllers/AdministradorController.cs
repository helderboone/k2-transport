﻿using K2.Web.Filters;
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
    public class AdministradorController : BaseController
    {
        public AdministradorController(IConfiguration configuration, ILogger<AdministradorController> logger, IHttpContextAccessor httpContextAccessor)
            : base(configuration, logger, httpContextAccessor)
        {

        }

        [Authorize(Policy = "Administrador")]
        [Route("administradores")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("listar-administradores")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as lista administradores cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarAdministradors(ProcurarUsuarioEntrada filtro)
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

            var apiResponse = await base.ChamarApi("usuarios/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            return new DatatablesResult(dataTablesParams.Draw, saida);
        }

        [Authorize(Policy = "Administrador")]
        [HttpGet]
        [Route("cadastrar-administrador")]
        public IActionResult CadastrarAdministrador()
        {
            return PartialView("Manter", null);
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("cadastrar-administrador")]
        public async Task<IActionResult> CadastrarAdministrador(CadastrarUsuarioEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do administrador não foram preenchidas.", new[] { "Verifique se todas as informações do administrador foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await base.ChamarApi("usuarios/cadastrar", Method.POST, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o administrador.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar o administrador.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = "Administrador")]
        [HttpGet]
        [Route("alterar-administrador/{id:int:min(1)}")]
        public async Task<IActionResult> AlterarAdministrador(int id)
        {
            var apiResponse = await base.ChamarApi("usuarios/obter-por-id/" + id, Method.GET);

            var saida = AdministradorSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações do administrador.", new[] { "A API não retornou nenhuma resposta." }));

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações do administrador.", saida.Mensagens));

            return PartialView("Manter", saida.ObterRetorno());
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("alterar-administrador")]
        public async Task<IActionResult> AlterarAdministrador(AlterarUsuarioEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do administrador não foram preenchidas.", new[] { "Verifique se todas as informações do motorista foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await base.ChamarApi("usuarios/alterar", Method.PUT, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar o administrador.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar o administrador.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("excluir-administrador/{id:int}")]
        public async Task<IActionResult> ExcluirAdministrador(int id)
        {
            var apiResponse = await base.ChamarApi("usuarios/excluir/" + id, Method.DELETE);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir o administrador.", new[] { "Não foi possível recuperar as informações do administrador." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir o administrador.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Administrador excluído com sucesso."));
        }
    }
}