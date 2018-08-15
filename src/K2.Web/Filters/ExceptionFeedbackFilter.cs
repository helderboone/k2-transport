using K2.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace K2.Web.Filters
{
    public class ExceptionFeedbackFilter : ExceptionFilterAttribute
    {
        private readonly TipoAcaoOcultarFeedback _tipoAcao;
        private readonly string _mensagem;
        private readonly string _mensagemAdicional;

        public ExceptionFeedbackFilter(TipoAcaoOcultarFeedback tipoAcao, string mensagem = null, string mensagemAdicional = null)
        {
            _tipoAcao          = tipoAcao;
            _mensagem          = mensagem;
            _mensagemAdicional = mensagemAdicional;
        }

        public override void OnException(ExceptionContext context)
        {
            var result = new ViewResult { ViewName = "Feedback" };

            var feedback = new FeedbackViewModel
            {
                Mensagem = _mensagem,
                MensagemAdicional = _mensagemAdicional,
                TipoAcao = _tipoAcao,
                Tipo = TipoFeedback.ERRO
            };

            result.ViewData.Add("Feedback", feedback);
            
            context.Result = result;
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            return base.OnExceptionAsync(context);
        }
    }
}
