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
        [Produces("application/json")]
        [HttpPost]
        [Route("autenticar")]
        [FeedbackExceptionFilter("Não foi possível realizar o login.", TipoAcaoOcultarFeedback.Ocultar, tipoResponse: TipoFeedbackResponse.Json)]
        public async Task<IActionResult> Login(string email, string senha, bool permanecerLogado)
        {
            var request = new RestRequest("v1/usuarios/autenticar", Method.POST);
            request.AddParameter("email", email);
            request.AddParameter("senha", senha);

            var response = await _restClient.ExecuteTaskAsync(request);

            if (response == null || string.IsNullOrEmpty(response.Content))
                return new JsonResult(new FeedbackViewModel(TipoFeedback.Erro, "Acesso negado.", new[] { "As informações retornadas pela API são nulas." }));

            var autenticacaoSaida = AutenticacaoSaida.Obter(response.Content);

            if (autenticacaoSaida == null)
                return new JsonResult(new FeedbackViewModel(TipoFeedback.Erro, "Acesso negado.", new[] { "As informações retornadas pela API são nulas." }));

            if (!autenticacaoSaida.Sucesso)
                return new JsonResult(new FeedbackViewModel(TipoFeedback.Atencao, "Acesso negado.", autenticacaoSaida.Mensagens));

            // Cria o cookie de autenticação

            var claims = new List<Claim>(autenticacaoSaida.ObterClaims());
            claims.Add(new Claim("jwtToken", autenticacaoSaida.ObterToken()));

            var userIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(userIdentity);

            var authenticationProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = permanecerLogado,
                ExpiresUtc = autenticacaoSaida.Retorno.DataExpiracaoToken
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);

            return new JsonResult(new FeedbackViewModel(TipoFeedback.Sucesso, "Usuário autenticado com sucesso."));
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
    }
}