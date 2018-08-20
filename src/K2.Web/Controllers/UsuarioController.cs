using K2.Web.Filters;
using K2.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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

        public IActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("")]
        [Route("login")]
        public IActionResult Login()
        {
            return User.Identity.IsAuthenticated
                ? (ActionResult)RedirectToAction("Index", "Inicio")
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

            if (response != null && !string.IsNullOrEmpty(response.Content))
            {
                var autenticacaoSaida = AutenticacaoSaida.Obter(response.Content);

                return autenticacaoSaida == null
                    ? new JsonResult(new FeedbackViewModel(TipoFeedback.Erro, "Não foi possível autenticar o usuário na API.", new[] { "As informações retornadas pela API são nulas." }))
                    : autenticacaoSaida.Sucesso
                        ? new JsonResult(new FeedbackViewModel(TipoFeedback.Sucesso, "Usuário autenticado com sucesso."))
                        : new JsonResult(new FeedbackViewModel(TipoFeedback.Atencao, "Não foi possível autenticar o usuário. Por favor, tenta novamente mais tarde.", autenticacaoSaida.Mensagens));
            }

            return new JsonResult(new FeedbackViewModel(TipoFeedback.Erro, "Não foi possível autenticar o usuário na API.", new[] { "As informações retornadas pela API são nulas." }));
        }
    }
}