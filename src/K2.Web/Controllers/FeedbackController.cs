using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace K2.Web.Controllers
{
    public class FeedbackController : Controller
    {
        [AllowAnonymous]
        [Route("feedback/{httpStatusCode:int}")]
        public IActionResult Feedback(HttpStatusCode httpStatusCode)
        {
            Feedback feedback;

            switch (httpStatusCode)
            {
                case HttpStatusCode.NotFound:
                case HttpStatusCode.BadRequest:
                    feedback = new Feedback(TipoFeedback.Atencao, "Página não encontrada.", new[] { $"({(int)httpStatusCode} - {httpStatusCode.ToString()})" }, tipoAcao: TipoAcaoAoOcultarFeedback.RedirecionarTelaInicial);
                    break;
                case HttpStatusCode.Forbidden:
                    feedback = new Feedback(TipoFeedback.Atencao, "Você não tem permissão para acessar essa funcionalidade.", new[] { $"({(int)httpStatusCode} - {httpStatusCode.ToString()})" }, tipoAcao: TipoAcaoAoOcultarFeedback.VoltarPaginaAnterior);
                    break;
                case HttpStatusCode.InternalServerError:
                    feedback = new Feedback(TipoFeedback.Erro, "Ooops! Um erro inesperado aconteceu...", new[] { $"{(int)httpStatusCode} - {httpStatusCode.ToString()}" }, tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar);
                    break;
                case HttpStatusCode.Unauthorized:
                    feedback = new Feedback(TipoFeedback.Atencao, "Você precisa realizar seu login antes de acessar essa funcionalidade.", new[] { $"({(int)httpStatusCode} - {httpStatusCode.ToString()})" }, tipoAcao: TipoAcaoAoOcultarFeedback.RedirecionarTelaLogin);
                    break;
                default:
                    feedback = new Feedback(TipoFeedback.Atencao, "Não foi possível acessar a página ou funcionalidade.", new[] { $"({(int)httpStatusCode} - {httpStatusCode.ToString()})" }, tipoAcao: TipoAcaoAoOcultarFeedback.VoltarPaginaAnterior);
                    break;
            }

            return new FeedbackResult(feedback);
        }
    }
}