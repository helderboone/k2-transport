﻿using JNogueira.Infraestrutura.Utilzao;
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

        [Authorize(Policy = "MotoristaOuProprietarioCarro")]
        [HttpGet]
        [Route("viagens-previstas")]
        public async Task<IActionResult> ViagensPrevistas()
        {
            var apiResponse = await _restSharpHelper.ChamarApi("viagens/obter-previstas", Method.GET);

            var saida = ViagensSaida.Obter(apiResponse.Content);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as viagens previstas.", saida.Mensagens, TipoAcaoAoOcultarFeedback.Ocultar));

            var registros = saida.Retorno;

            return View(registros);
        }

        //[Authorize(Policy = TipoPerfil.Administrador)]
        //[HttpPost]
        //[Route("listar-localidades")]
        //[FeedbackExceptionFilter("Ocorreu um erro ao obter as lista localidades cadastradas.", TipoAcaoAoOcultarFeedback.Ocultar)]
        //public async Task<IActionResult> ListarViagems(ProcurarViagemEntrada filtro)
        //{
        //    if (filtro == null)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações para a procura não foram preenchidas.", tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));

        //    filtro.OrdenarPor     = _datatablesHelper.OrdenarPor;
        //    filtro.OrdenarSentido = _datatablesHelper.OrdenarSentido;
        //    filtro.PaginaIndex    = _datatablesHelper.PaginaIndex;
        //    filtro.PaginaTamanho  = _datatablesHelper.PaginaTamanho;

        //    var parametros = new Parameter[]
        //    {
        //        new Parameter{ Name = "filtro", Value = filtro.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
        //    };

        //    var apiResponse = await _restSharpHelper.ChamarApi("localidades/procurar", Method.POST, parametros);

        //    var saida = ProcurarSaida.Obter(apiResponse.Content);

        //    if (!saida.Sucesso)
        //        return new DatatablesResult(_datatablesHelper.Draw, saida);

        //    var registros = saida.ObterRegistros<ViagemRegistro>();

        //    var lst = registros.Select(x => new
        //    {
        //        id = x.Id,
        //        nome = x.Nome,
        //        uf = x.Uf.ObterNomeUfPorSiglaUf()
        //    }).ToList();

        //    return new DatatablesResult(_datatablesHelper.Draw, lst.Count, lst);
        //}

        //[Authorize(Policy = TipoPerfil.Administrador)]
        //[HttpGet]
        //[Route("cadastrar-localidade")]
        //public IActionResult CadastrarViagem()
        //{
        //    return PartialView("Manter", null);
        //}

        //[Authorize(Policy = TipoPerfil.Administrador)]
        //[HttpPost]
        //[Route("cadastrar-localidade")]
        //public async Task<IActionResult> CadastrarViagem(CadastrarViagemEntrada entrada)
        //{
        //    if (entrada == null)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do localidade não foram preenchidas.", new[] { "Verifique se todas as informações do localidade foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

        //    var parametros = new Parameter[]
        //    {
        //        new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
        //    };

        //    var apiResponse = await _restSharpHelper.ChamarApi("localidades/cadastrar", Method.POST, parametros);

        //    var saida = ViagemSaida.Obter(apiResponse.Content);

        //    if (saida == null)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar a localidade.", new[] { "A API não retornou nenhuma resposta." }));

        //    return !saida.Sucesso
        //        ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar a localidade.", saida.Mensagens))
        //        : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        //}

        //[Authorize(Policy = TipoPerfil.Administrador)]
        //[HttpGet]
        //[Route("alterar-localidade/{id:int:min(1)}")]
        //public async Task<IActionResult> AlterarViagem(int id)
        //{
        //    var apiResponse = await _restSharpHelper.ChamarApi("localidades/obter-por-id/" + id, Method.GET);

        //    var saida = ViagemSaida.Obter(apiResponse.Content);

        //    if (saida == null)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações da localidade.", new[] { "A API não retornou nenhuma resposta." }));

        //    if (!saida.Sucesso)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível exibir as informações da localidade.", saida.Mensagens));

        //    return PartialView("Manter", saida.Retorno);
        //}

        //[Authorize(Policy = TipoPerfil.Administrador)]
        //[HttpPost]
        //[Route("alterar-localidade")]
        //public async Task<IActionResult> AlterarViagem(AlterarViagemEntrada entrada)
        //{
        //    if (entrada == null)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da localidade não foram preenchidas.", new[] { "Verifique se todas as informações da localidade foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

        //    var parametros = new Parameter[]
        //    {
        //        new Parameter{ Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" }
        //    };

        //    var apiResponse = await _restSharpHelper.ChamarApi("localidades/alterar", Method.PUT, parametros);

        //    var saida = Saida.Obter(apiResponse.Content);

        //    if (saida == null)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar a localidade.", new[] { "A API não retornou nenhuma resposta." }));

        //    return !saida.Sucesso
        //        ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar a localidade.", saida.Mensagens))
        //        : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        //}

        //[Authorize(Policy = TipoPerfil.Administrador)]
        //[HttpPost]
        //[Route("excluir-localidade/{id:int}")]
        //public async Task<IActionResult> ExcluirViagem(int id)
        //{
        //    var apiResponse = await _restSharpHelper.ChamarApi("localidades/excluir/" + id, Method.DELETE);

        //    var saida = Saida.Obter(apiResponse.Content);

        //    if (saida == null)
        //        return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir a localidade.", new[] { "A API não retornou nenhuma resposta." }));

        //    return !saida.Sucesso
        //        ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir a localidade.", saida.Mensagens))
        //        : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Viagem excluída com sucesso."));
        //}
    }
}