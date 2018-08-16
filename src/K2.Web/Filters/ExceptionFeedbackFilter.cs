using K2.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace K2.Web.Filters
{
    public class ExceptionFeedbackHtmlFilter : ExceptionFilterAttribute
    {
        private readonly string _mensagem;
        private readonly TipoAcaoOcultarFeedback _tipoAcaoOcultar;
        private readonly string _mensagemAdicional;

        public ExceptionFeedbackHtmlFilter(string mensagem, TipoAcaoOcultarFeedback tipoAcaoOcultar, string mensagemAdicional = null)
        {
            _mensagem = mensagem;
            _tipoAcaoOcultar = tipoAcaoOcultar;
            _mensagemAdicional = mensagemAdicional;
        }

        public override void OnException(ExceptionContext context)
        {
            var result = new ViewResult { ViewName = "Feedback" };

            var feedback = new FeedbackViewModel
            {
                Mensagem = _mensagem,
                MensagemAdicional = context.Exception.GetBaseException().Message,
                TipoAcao = _tipoAcaoOcultar,
                Tipo = TipoFeedback.ERRO
            };
            result.ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState);
            result.ViewData.Model = feedback;

            context.Result = result;
        }
    }

    //public class ExceptionFeedbackJsonFilter : ExceptionFilterAttribute
    //{
    //    public override void OnException(ExceptionContext context)
    //    {
    //        base.OnException(context);
    //    }
    //}

    //public class ExceptionFeedbackFilter : ExceptionFilterAttribute
    //{
    //    private readonly TipoAcaoOcultarFeedback _tipoAcao;
    //    private readonly string _mensagem;
    //    private readonly string _mensagemAdicional;

    //    public ExceptionFeedbackFilter(TipoAcaoOcultarFeedback tipoAcao, string mensagem = null, string mensagemAdicional = null)
    //    {
    //        _tipoAcao          = tipoAcao;
    //        _mensagem          = mensagem;
    //        _mensagemAdicional = mensagemAdicional;
    //    }

    //    public ExceptionFeedbackFilter(TipoAcaoOcultarFeedback tipoAcao, string mensagem = null, string mensagemAdicional = null)
    //    {
    //        _tipoAcao = tipoAcao;
    //        _mensagem = mensagem;
    //        _mensagemAdicional = mensagemAdicional;
    //    }

    //    public override void OnException(ExceptionContext context)
    //    {
    //        var result = new ViewResult { ViewName = "Feedback" };

    //        var feedback = new FeedbackViewModel
    //        {
    //            Mensagem = _mensagem,
    //            MensagemAdicional = _mensagemAdicional,
    //            TipoAcao = _tipoAcao,
    //            Tipo = TipoFeedback.ERRO
    //        };

    //        result.ViewData.Add("Feedback", feedback);
            
    //        context.Result = result;
    //    }

    //    public override Task OnExceptionAsync(ExceptionContext context)
    //    {
    //        return base.OnExceptionAsync(context);
    //    }
    //}
}
