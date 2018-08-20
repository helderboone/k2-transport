using K2.Web.Filters;
using K2.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
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
        [HttpPost]
        [Route("login")]
        [FeedbackExceptionFilter("Não foi possível realizar o login.", TipoAcaoOcultarFeedback.OcultarPopups, tipoResponse: TipoFeedbackResponse.Json)]
        public async Task<IActionResult> Login(string email, string senha, bool permanecerLogado)
        {
            var request = new RestRequest("v1/usuarios/autenticar", Method.POST);
            request.AddParameter("email", email);
            request.AddParameter("senha", senha);

            var response = await _apiClient.ExecuteTaskAsync<Saida>(request);

            if (response != null && response.Data.Sucesso)
            {
                // Criar cookie de autenticação
                var jwtTokenSaida = (JwtTokenSaida)response.Data.Retorno;
                return new EmptyResult();
            }
            else
            {
                var feedback = new FeedbackViewModel(TipoFeedback.ATENCAO, response.Data.Mensagens, null, TipoAcaoOcultarFeedback.OcultarPopups);

                return new JsonResult(feedback);
            }
        }
    }
}