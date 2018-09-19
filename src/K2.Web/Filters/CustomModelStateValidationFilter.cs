using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace K2.Web.Filters
{
    /// <summary>
    /// Filtro que extrai as mensagens do ModelState e coloca no padrão de saida da API.
    /// </summary>
    public class CustomModelStateValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var lstErros = new List<string>();

                foreach (var modelState in context.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        lstErros.Add(error.Exception != null ? error.Exception.Message : error.ErrorMessage);
                    }
                }

                context.Result = new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Por favor corrija os erros abaixo:", lstErros));
            }
        }
    }
}
