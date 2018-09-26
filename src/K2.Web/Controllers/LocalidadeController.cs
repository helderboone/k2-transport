﻿using JNogueira.Infraestrutura.Utilzao;
using K2.Web.Filters;
using K2.Web.Helpers;
using K2.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Linq;
using System.Threading.Tasks;

namespace K2.Web.Controllers
{
    public class LocalidadeController : BaseController
    {
        public LocalidadeController(IConfiguration configuration, ILogger<LocalidadeController> logger, IHttpContextAccessor httpContextAccessor)
            : base(configuration, logger, httpContextAccessor)
        {

        }

        [Authorize(Policy = "Administrador")]
        [Route("localidades")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("listar-localidades")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as lista localidades cadastradas.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarLocalidades(ProcurarLocalidadeEntrada filtro)
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

            var apiResponse = await base.ChamarApi("localidades/procurar", Method.POST, parametros);

            var saida = ProcurarSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new DatatablesResult(dataTablesParams.Draw, saida);

            var registros = saida.ObterRetorno().Registros;

            var lst = registros.Select(x => new
            {
                id = ((JObject)x)["id"].Value<int>(),
                nome = ((JObject)x)["nome"].Value<string>(),
                uf = ((JObject)x)["uf"].Value<string>().ObterNomeUfPorSiglaUf()
            }).ToList();

            return new DatatablesResult(dataTablesParams.Draw, lst.Count, lst);
        }

        [Authorize(Policy = "Administrador")]
        [HttpGet]
        [Route("cadastrar-localidade")]
        public IActionResult CadastrarLocalidade()
        {
            return PartialView("Manter", null);
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("cadastrar-localidade")]
        public async Task<IActionResult> CadastrarLocalidade(CadastrarLocalidadeEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do localidade não foram preenchidas.", new[] { "Verifique se todas as informações do localidade foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await base.ChamarApi("localidades/cadastrar", Method.POST, parametros);

            var saida = LocalidadeSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar a localidade.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar a localidade.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = "Administrador")]
        [HttpGet]
        [Route("alterar-localidade/{id:int:min(1)}")]
        public async Task<IActionResult> AlterarLocalidade(int id)
        {
            var apiResponse = await base.ChamarApi("localidades/obter-por-id/" + id, Method.GET);

            var saida = LocalidadeSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações da localidade.", new[] { "A API não retornou nenhuma resposta." }));

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações da localidade.", saida.Mensagens));

            return PartialView("Manter", saida.ObterRetorno());
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("alterar-localidade")]
        public async Task<IActionResult> AlterarLocalidade(AlterarLocalidadeEntrada entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da localidade não foram preenchidas.", new[] { "Verifique se todas as informações da localidade foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var parametros = new Parameter[]
            {
                new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
            };

            var apiResponse = await base.ChamarApi("localidades/alterar", Method.PUT, parametros);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar a localidade.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar a localidade.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("excluir-localidade/{id:int}")]
        public async Task<IActionResult> ExcluirLocalidade(int id)
        {
            var apiResponse = await base.ChamarApi("localidades/excluir/" + id, Method.DELETE);

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir a localidade.", new[] { "A API não retornou nenhuma resposta." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir a localidade.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Localidade excluída com sucesso."));
        }
    }
}