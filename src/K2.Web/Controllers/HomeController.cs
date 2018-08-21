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

        //public IActionResult About()
        //{
        //    ViewData["Message"] = "Your application description page.";

        //    return View();
        //}

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
