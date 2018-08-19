using Microsoft.AspNetCore.Mvc;

namespace K2.Web.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
            
        }

        //[Route("")]
        //[FeedbackExceptionFilter("mensagem aqui", TipoAcaoOcultarFeedback.FecharJanela, "mensagem adicional")]
        //public IActionResult Index()
        //{
        //    var b = 0;
        //    var i = 5 / b;

        //    return View("About");
        //}

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
