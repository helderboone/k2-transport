using K2.Dominio.Comandos.Saida;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace K2.Api.Controllers
{
    public class FeedbackController : Controller
    {
        [AllowAnonymous]
        [Route("feedback/{httpStatusCode:int}")]
        public IActionResult Feedback(HttpStatusCode httpStatusCode)
        {
            Saida saida;

            switch (httpStatusCode)
            {
                case HttpStatusCode.NotFound:
                case HttpStatusCode.BadRequest:
                    saida = new NotFoundApiResponse(HttpContext.Request.Path);
                    break;
                case HttpStatusCode.Forbidden:
                    saida = new ForbiddenApiResponse();
                    break;
                case HttpStatusCode.InternalServerError:
                    var ex = HttpContext.Features.Get<IExceptionHandlerFeature>();
                    saida = new InternalServerErrorApiResponse(ex.Error);
                    break;
                case HttpStatusCode.Unauthorized:
                    saida = new UnauthorizedApiResponse();
                    break;
                default:
                    saida = new Saida(false, new[] { $"Erro {(int)httpStatusCode}: {httpStatusCode.ToString()}" }, null);
                    break;
            }

            return new FeedbackResult(saida);
        }
    }
}