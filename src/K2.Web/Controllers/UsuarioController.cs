using K2.Api.Models;
using K2.Web.Filters;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace K2.Web.Controllers
{
    [Authorize]
    public class UsuarioController : BaseController
    {
        public UsuarioController(IConfiguration configuration, ILogger<UsuarioController> logger, IHttpContextAccessor httpContextAccessor)
            : base(configuration, logger, httpContextAccessor)
        {
            
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        [Route("login")]
        public IActionResult Login()
        {
            return User.Identity.IsAuthenticated
                ? (ActionResult)RedirectToAction("Index", "Home")
                : View();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [FeedbackExceptionFilter("Ocorreu um erro na tentativa de efetuar login.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> Login(string email, string senha, bool permanecerLogado)
        {
            var parametros = new Parameter[]
            {
                new Parameter{ Name = "email", Value = email, Type = ParameterType.QueryString },
                new Parameter{ Name = "senha", Value = senha, Type = ParameterType.QueryString }
            };

            var apiResponse = await base.ChamarApi("usuarios/autenticar", Method.POST, parametros, false);

            var saida = AutenticacaoSaida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível efetuar o login.", new[] { "Não foi possível recuperar as informações do usuário." }));

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível efetuar o login.", saida.Mensagens));

            // Cria o cookie de autenticação

            var claims = new List<Claim>(saida.ObterClaims());
            claims.Add(new Claim("jwtToken", saida.ObterToken()));

            var userIdentity = new ClaimsIdentity(
                new GenericIdentity(saida.ObterNomeUsuario()),
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme,
                null,
                null);

            var principal = new ClaimsPrincipal(userIdentity);

            var authenticationProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = permanecerLogado,
                ExpiresUtc = saida.ObterRetorno().DataExpiracaoToken
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Usuário autenticado com sucesso."));
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return new EmptyResult();
        }

        [HttpGet]
        [Route("alterar-senha")]
        public IActionResult AlterarSenha()
        {
            return PartialView();
        }

        [HttpPost]
        [Route("alterar-senha")]
        [FeedbackExceptionFilter("Ocorreu um erro na tentativa de alterar a senha de acesso.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarSenha(string senhaAtual, string senhaNova, string confirmacaoSenhaNova, bool enviarEmailSenhaNova)
        {
            var entrada = new AlterarSenhaUsuarioEntrada
            {
                SenhaAtual           = senhaAtual,
                SenhaNova            = senhaNova,
                ConfirmacaoSenhaNova = confirmacaoSenhaNova,
                EnviarEmailSenhaNova = enviarEmailSenhaNova
            };

            var apiResponse = await base.ChamarApi("usuarios/alterar-senha", Method.PUT, new[] { new Parameter { Name = "model", Value = entrada.ObterJson(), Type = ParameterType.RequestBody, ContentType = "application/json" } });

            var saida = Saida.Obter(apiResponse.Content);

            if (saida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar sua senha de acesso.", new[] { "Não foi possível recuperar as informações do usuário." }));

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar sua senha de acesso.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }
    }
}