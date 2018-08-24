using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace K2.Web.Controllers
{
    public class FeedbackController : Controller
    {
        [AllowAnonymous]
        [HttpGet]
        [Route("feedback/{httpStatusCode:int}")]
        public IActionResult Feedback(HttpStatusCode httpStatusCode)
        {
            Feedback feedback;

            var tipoResponse = HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest"
                ? TipoFeedbackResponse.Json
                : TipoFeedbackResponse.Html;

            switch (httpStatusCode)
            {
                case HttpStatusCode.NotFound:
                case HttpStatusCode.BadRequest:
                    feedback = new Feedback(TipoFeedback.Atencao, $"Página não encontrada. ({httpStatusCode.ToString()} - {(int)httpStatusCode})", tipoAcao: TipoAcaoAoOcultarFeedback.RedirecionarTelaInicial);
                    break;
                case HttpStatusCode.Forbidden:
                    feedback = new Feedback(TipoFeedback.Atencao, $"Você não tem permissão para acessar essa funcionalidade. ({httpStatusCode.ToString()} - {(int)httpStatusCode})", tipoAcao: TipoAcaoAoOcultarFeedback.VoltarPaginaAnterior);
                    break;
                case HttpStatusCode.InternalServerError:
                    var exception = HttpContext.Features.Get<IExceptionHandlerFeature>();
                    feedback = new Feedback(TipoFeedback.Erro, $"Ooops! Um erro inesperado aconteceu... ({httpStatusCode.ToString()} - {(int)httpStatusCode})", tipoAcao: TipoAcaoAoOcultarFeedback.VoltarPaginaAnterior);
                    break;
                case HttpStatusCode.Unauthorized:
                    feedback = new Feedback(TipoFeedback.Atencao, $"Você precisa realizar seu login antes de acessar essa  funcionalidade. ({httpStatusCode.ToString()}).", tipoAcao: TipoAcaoAoOcultarFeedback.RedirecionarTelaLogin);
                    break;
                default:
                    feedback = new Feedback(TipoFeedback.Atencao, $"Não foi possível acessar a página ou funcionalidade. ({httpStatusCode.ToString()} - {(int)httpStatusCode})", tipoAcao: TipoAcaoAoOcultarFeedback.VoltarPaginaAnterior);
                    break;
            }

            return new FeedbackResult(feedback, tipoResponse);
        }
    }
}