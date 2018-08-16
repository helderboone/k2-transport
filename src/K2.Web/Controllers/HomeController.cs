using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using K2.Web.Models;
using K2.Web.Filters;

namespace K2.Web.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        [ExceptionFeedbackHtmlFilter("Deu erro aqui", TipoAcaoOcultarFeedback.RedirecionarTelaInicial)]
        public IActionResult Index()
        {
            var b = 0;
            var i = 5 / b;

            return View("About");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
