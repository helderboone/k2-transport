using K2.Web.Filters;
using K2.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace K2.Web.Controllers
{
    [Authorize]
    public class UsuarioController : BaseController
    {
        public UsuarioController(IConfiguration configuration)
            : base(configuration)
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
        [Route("autenticar")]
        [FeedbackExceptionFilter("Ocorreu um erro na tentativa de efetuar o login.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> Login(string email, string senha, bool permanecerLogado)
        {
            var request = new RestRequest("v1/usuarios/autenticar", Method.POST);
            request.AddParameter("email", email);
            request.AddParameter("senha", senha);

            var response = await _restClient.ExecuteTaskAsync(request);

            if (response == null || string.IsNullOrEmpty(response.Content))
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível efetuar o login.", new[] { "Não foi possível recuperar as informações do usuário.", "Provavelmente a API está offline." }), TipoFeedbackResponse.Json);

            var autenticacaoSaida = AutenticacaoSaida.Obter(response.Content);

            if (autenticacaoSaida == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível efetuar o login.", new[] { "Não foi possível recuperar as informações do usuário." }), TipoFeedbackResponse.Json);

            if (!autenticacaoSaida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível efetuar o login.", autenticacaoSaida.Mensagens), TipoFeedbackResponse.Json);

            // Cria o cookie de autenticação

            var claims = new List<Claim>(autenticacaoSaida.ObterClaims());
            claims.Add(new Claim("jwtToken", autenticacaoSaida.ObterToken()));

            var userIdentity = new ClaimsIdentity(
                new GenericIdentity(autenticacaoSaida.ObterNomeUsuario()),
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme,
                null,
                null);

            var principal = new ClaimsPrincipal(userIdentity);

            var authenticationProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = permanecerLogado,
                ExpiresUtc = autenticacaoSaida.Retorno.DataExpiracaoToken
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Usuário autenticado com sucesso."), TipoFeedbackResponse.Json);
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
        //[FeedbackExceptionFilter("Não foi possível realizar o login.", TipoAcaoAoOcultarFeedback.Ocultar, tipoResponse: TipoFeedbackResponse.Json)]
        public IActionResult AlterarSenha()
        {
            return PartialView();
        }
    }
}