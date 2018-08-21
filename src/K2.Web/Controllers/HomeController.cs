using K2.Web.Filters;
using K2.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace K2.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public HomeController()
        {
            
        }

        [Route("inicio")]
        //[FeedbackExceptionFilter("mensagem aqui", TipoAcaoOcultarFeedback.FecharJanela, "mensagem adicional")]
        public IActionResult Index()
        {
            return View("About");
        }

        [Authorize]
        [HttpGet]
        [Route("acesso-negado")]
        public IActionResult AcessoNegado()
        {
            var feedback = new FeedbackViewModel(TipoFeedback.Atencao, "Você não tem permissão para acessar essa página.", tipoAcao: TipoAcaoOcultarFeedback.RedirecionarTelaInicial);

            return View("Feedback", feedback);
        }



        [Authorize(Policy = "pepeca")]
        [Route("about")]
        [FeedbackExceptionFilter("Não foi possível realizar o login.", TipoAcaoOcultarFeedback.Ocultar, tipoResponse: TipoFeedbackResponse.Json)]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        //public IActionResult Contact()
        //{
        //    ViewData["Message"] = "Your contact page.";

        //    return View();
        //}

        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
