using K2.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace K2.Web.Controllers
{
    public class BaseController : Controller
    {
       
        /// <summary>
        /// Redireciona para a página de mensagem
        /// </summary>
        public IActionResult ExibirFeedback(Exception ex, TipoAcaoOcultarFeedback tipoAcao, string mensagem = null)
        {
            //if (ex is CookieAutenticacaoNaoEncontradoException)
            //{
            //    return ExibirMensagem(new MensagemViewModel
            //    {
            //        Tipo = TipoMensagem.Aviso,
            //        Titulo = "Login expirado",
            //        Mensagem = "Sua sessão expirou. Você precisa efetuar seu login novamente.",
            //        MensagemAdicional = "Ao clicar no botão \"FECHAR\" você será redirecionado para a tela de login.",
            //        TipoAcao = TipoAcaoOcultarMensagem.RedirecionarTelaLogin
            //    });
            //}

            //if (Common.ObterAmbiente() == Common.TipoAmbiente.Producao)
            //{
            //    var infoAdicional = new List<KeyValuePair<string, string>>
            //    {
            //        new KeyValuePair<string, string>("Usuário", CookieAutenticacaoHelper.ObterNomeUsuario())
            //    };

            //    var slackProxy = new SlackProxy(ConfigurationManager.AppSettings["Slack.Webhook.URL"]);

            //    slackProxy.Postar(new Mensagem(ConfigurationManager.AppSettings["Slack.Canal"], mensagem, ConfigurationManager.AppSettings["Slack.UserName"]), ex, infoAdicional);
            //}

            //Elmah.ErrorSignal.FromCurrentContext().Raise(!string.IsNullOrEmpty(mensagem) ? new Exception(mensagem, ex) : ex);

            return null;

            //return ExibirFeedback(new FeedbackViewModel
            //{
            //    Tipo = TipoFeedback.ERRO,
            //    //Titulo = "Ooooops...",
            //    Mensagem = string.IsNullOrEmpty(mensagem) ? ex.GetBaseException().Message : mensagem,
            //    MensagemAdicional = "Todas as informações sobre o erro ocorrido foram registradas. A causa do erro será investigada e em breve esse problema será resolvido. Pedimos desculpas pelo transtorno.",
            //    TipoAcao = tipoAcao
            //});
        }

        /// <summary>
        /// Redireciona para a página de mensagem
        /// </summary>
        public IActionResult ExibirFeedback(FeedbackViewModel feedback)
        {
            return View("Mensagem", feedback);
        }

        /// <summary>
        /// Retorna as informações de uma mensagem em formato Json
        /// </summary>
        public JsonResult ObterFeedback(Exception ex, TipoAcaoOcultarFeedback tipoAcao, string mensagem = null)
        {
            //if (ex is CookieAutenticacaoNaoEncontradoException)
            //{
            //    return RetornarJsonMensagem(new MensagemViewModel
            //    {
            //        Tipo = TipoMensagem.Aviso,
            //        Titulo = "Login expirado",
            //        Mensagem = "Sua sessão expirou. Você precisa fazer seu login novamente.",
            //        MensagemAdicional = "Ao clicar no botão \"FECHAR\" você será redirecionado para a tela de login.",
            //        TipoAcao = TipoAcaoOcultarMensagem.RedirecionarTelaLogin
            //    });
            //}

            //if (Common.ObterAmbiente() == Common.TipoAmbiente.Producao)
            //{
            //    var infoAdicional = new List<KeyValuePair<string, string>>
            //    {
            //        new KeyValuePair<string, string>("Usuário", CookieAutenticacaoHelper.ObterNomeUsuario())
            //    };

            //    var slackProxy = new SlackProxy(ConfigurationManager.AppSettings["Slack.Webhook.URL"]);

            //    slackProxy.Postar(new Mensagem(ConfigurationManager.AppSettings["Slack.Canal"], mensagem, ConfigurationManager.AppSettings["Slack.UserName"]), ex, infoAdicional);
            //}

            //Elmah.ErrorSignal.FromCurrentContext().Raise(!string.IsNullOrEmpty(mensagem) ? new Exception(mensagem, ex) : ex);

            return null;
            
            //return ObterFeedback(new FeedbackViewModel
            //{
            //    Tipo = TipoFeedback.ERRO,
            //    //Titulo = "Ooooops...",
            //    Mensagem = string.IsNullOrEmpty(mensagem) ? ex.GetBaseException().Message : mensagem,
            //    MensagemAdicional = "Todas as informações sobre o erro ocorrido foram registradas. A causa do erro será investigada e em breve esse problema será resolvido. Pedimos desculpas pelo transtorno.",
            //    TipoAcao = tipoAcao
            //});
        }

        /// <summary>
        /// Retorna as informações de uma mensagem em formato Json
        /// </summary>
        public JsonResult ObterFeedback(FeedbackViewModel feedback)
        {
            return Json(new { feedback.Mensagem, Tipo = (int)feedback.Tipo, TipoAcao = (int)feedback.TipoAcao, feedback.MensagemAdicional });
        }
    }
}
