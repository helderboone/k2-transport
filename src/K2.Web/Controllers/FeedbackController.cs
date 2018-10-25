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
                    feedback = new Feedback(TipoFeedback.Atencao, "Página não encontrada.", tipoAcao: TipoAcaoAoOcultarFeedback.RedirecionarTelaInicial);
                    break;
                case HttpStatusCode.Forbidden:
                    feedback = new Feedback(TipoFeedback.Atencao, "Você não tem permissão para acessar essa funcionalidade.", tipoAcao: TipoAcaoAoOcultarFeedback.RedirecionarTelaInicial);
                    break;
                case HttpStatusCode.InternalServerError:
                    feedback = new Feedback(TipoFeedback.Erro, "Ooops! Um erro inesperado aconteceu...", new[] { "A ocorrência desse erro foi registrada e será posteriormente analisada para identificar a causa. Pedimos desculpas pelo transtorno." }, tipoAcao: TipoAcaoAoOcultarFeedback.VoltarPaginaAnterior);
                    break;
                case HttpStatusCode.Unauthorized:
                    feedback = new Feedback(TipoFeedback.Atencao, "Você precisa realizar seu login antes de acessar essa funcionalidade.", tipoAcao: TipoAcaoAoOcultarFeedback.RedirecionarTelaLogin);
                    break;
                default:
                    feedback = new Feedback(TipoFeedback.Atencao, "Não foi possível acessar a página ou funcionalidade.", tipoAcao: TipoAcaoAoOcultarFeedback.RedirecionarTelaInicial);
                    break;
            }

            return new FeedbackResult(feedback);
        }
    }
}