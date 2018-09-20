using K2.Dominio.Comandos.Saida;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace K2.Api.Filters
{
    /// <summary>
    /// Filtro que extrai as mensagens do ModelState e coloca no padrão de saida da API.
    /// </summary>
    public class CustomModelStateValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new JsonResult(new Saida(false, new[] { "Erro na validação do parâmetro esperado pela rota (ModelState)." }, new ValidationResultModel(context.ModelState)));
        }
    }

    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Campo { get; }

        public string Mensagem { get; }

        public ValidationError(string field, string message)
        {
            Campo = field != string.Empty ? field : null;
            Mensagem = message;
        }
    }

    public class ValidationResultModel
    {
        public List<ValidationError> Erros { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Erros = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, string.IsNullOrEmpty(x.ErrorMessage) ? x.Exception?.Message : x.ErrorMessage)))
                    .ToList();
        }
    }
}
