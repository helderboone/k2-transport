using K2.Web.Filters;
using K2.Web.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace K2.Web.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _urlApi;

        private readonly RestClient _apiClient;

        public UsuarioController(IConfiguration configuration)
        {
            _configuration = configuration;

            _urlApi = configuration["UrlApi"];
            _apiClient = new RestClient(_urlApi);
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
        public async Task<IActionResult> Autenticar(string email, string senha, bool permanecerLogado)
        {
            var request = new RestRequest("v1/usuarios/autenticar", Method.POST);
            request.AddParameter("email", email);
            request.AddParameter("senha", senha);

            var response = await _apiClient.ExecuteTaskAsync(request);

            if (response == null || string.IsNullOrEmpty(response.Content))
                return new JsonResult(new FeedbackViewModel(TipoFeedback.Erro, "Acesso negado.", new[] { "As informações retornadas pela API são nulas." }));

            var autenticacaoSaida = AutenticacaoSaida.Obter(response.Content);

            if (autenticacaoSaida == null)
                return new JsonResult(new FeedbackViewModel(TipoFeedback.Erro, "Acesso negado.", new[] { "As informações retornadas pela API são nulas." }));

            if (!autenticacaoSaida.Sucesso)
                return new JsonResult(new FeedbackViewModel(TipoFeedback.Atencao, "Acesso negado.", autenticacaoSaida.Mensagens));

            // Cria o cookie de autenticação

            var userIdentity = new ClaimsIdentity(autenticacaoSaida.ObterClaims(), CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(userIdentity);

            var authenticationProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = permanecerLogado,
                ExpiresUtc = DateTime.UtcNow.AddDays(365 * 10)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authenticationProperties);

            return new JsonResult(new FeedbackViewModel(TipoFeedback.Sucesso, "Usuário autenticado com sucesso."));
        }
    }
}