using K2.Dominio.Comandos.Saida;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace K2.Api
{
    public class FeedbackResult : IActionResult
    {
        private readonly Saida _saida;

        public FeedbackResult(Saida saida)
        {
            _saida = saida;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var jsonResult = new JsonResult(_saida);

            await jsonResult.ExecuteResultAsync(context);
        }
    }
}
