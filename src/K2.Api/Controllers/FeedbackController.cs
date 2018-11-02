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
        [Route("/")]
        public IActionResult Hello()
        {
            return Content("API K2 Transport");
        }

        [AllowAnonymous]
        [Route("feedback/{httpStatusCode:int}")]
        public IActionResult Feedback(HttpStatusCode httpStatusCode)
        {
            Saida saida;

            switch (httpStatusCode)
            {
                case HttpStatusCode.NotFound:
                case HttpStatusCode.BadRequest:
                    var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                    saida = new Saida(false, new[] { $"Erro 404: O endereço \"{feature?.OriginalPath}\" não foi encontrado." }, null);
                    break;
                case HttpStatusCode.Forbidden:
                    saida = new Saida(false, new[] { "Erro 403: Acesso negado. Você não tem permissão de acesso para essa funcionalidade." }, null);
                    break;
                case HttpStatusCode.InternalServerError:
                    var ex = HttpContext.Features.Get<IExceptionHandlerFeature>();
                    saida = new Saida(false, new[] { "Erro 500: " + ex.Error.Message }, new { Exception = ex.Error.Message, BaseException = ex.Error.GetBaseException().Message, ex.Error.Source });
                    break;
                case HttpStatusCode.Unauthorized:
                    saida = new Saida(false, new[] { "Erro 401: Acesso negado. Certifique-se que você foi autenticado." }, null);
                    break;
                default:
                    saida = new Saida(false, new[] { $"Erro {(int)httpStatusCode}: {httpStatusCode.ToString()}" }, null);
                    break;
            }

            return new FeedbackResult(saida);
        }
    }
}