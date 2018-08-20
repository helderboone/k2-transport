using K2.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text;

namespace K2.Web.Filters
{
    public class FeedbackExceptionFilterAttribute : TypeFilterAttribute
    {
        public FeedbackExceptionFilterAttribute(string mensagem, TipoAcaoOcultarFeedback tipoAcaoOcultar, string mensagemAdicional = "", TipoFeedbackResponse tipoResponse = TipoFeedbackResponse.Html) 
            : base(typeof(FeedbackExceptionFilter))
        {
            Arguments = new object[] { mensagem, tipoAcaoOcultar, mensagemAdicional, tipoResponse };
        }

        private class FeedbackExceptionFilter : ExceptionFilterAttribute
        {
            private readonly IHostingEnvironment _hostingEnvironment;

            private readonly string _mensagem;
            private readonly TipoAcaoOcultarFeedback _tipoAcaoOcultar;
            private readonly string _mensagemAdicional;
            private readonly TipoFeedbackResponse _tipoResponse;

            public FeedbackExceptionFilter(IHostingEnvironment hostingEnvironment, string mensagem, TipoAcaoOcultarFeedback tipoAcaoOcultar, string mensagemAdicional, TipoFeedbackResponse tipoResponse = TipoFeedbackResponse.Html)
            {
                _hostingEnvironment = hostingEnvironment;

                _mensagem = mensagem;
                _tipoAcaoOcultar = tipoAcaoOcultar;
                _mensagemAdicional = mensagemAdicional;
                _tipoResponse = tipoResponse;
            }

            public override void OnException(ExceptionContext context)
            {
                IActionResult result = null;

                FeedbackViewModel feedbackViewModel;

                switch (_tipoResponse)
                {
                    case TipoFeedbackResponse.Json:

                        if (_hostingEnvironment.IsDevelopment())
                        {
                            var html = new StringBuilder();

                            if (!string.IsNullOrEmpty(_mensagemAdicional))
                            {
                                html.Append("<p>" + _mensagemAdicional + "</p>");
                            }

                            html.Append($"<p>Exception: {context.Exception.Message}</p>");

                            if (context.Exception.Message != context.Exception.GetBaseException().Message)
                                html.Append($"<p>Base exception: {context.Exception.GetBaseException().Message}</p>");

                            feedbackViewModel = new FeedbackViewModel(TipoFeedback.Erro, _mensagem, new[] { html.ToString() }, _tipoAcaoOcultar);
                        }
                        else
                        {
                            feedbackViewModel = new FeedbackViewModel(TipoFeedback.Erro, _mensagem, new[] { _mensagemAdicional }, _tipoAcaoOcultar);
                        }

                        result = new JsonResult(feedbackViewModel);

                        break;
                    case TipoFeedbackResponse.Html:

                        if (_hostingEnvironment.IsDevelopment())
                        {
                            var html = new StringBuilder();

                            if (!string.IsNullOrEmpty(_mensagemAdicional))
                            {
                                html.Append("<p>" + _mensagemAdicional + "</p>");
                            }

                            html.Append($"<p>Exception: {context.Exception.Message}</p>");

                            if (context.Exception.Message != context.Exception.GetBaseException().Message)
                                html.Append($"<p>Base exception: {context.Exception.GetBaseException().Message}</p>");

                            feedbackViewModel = new FeedbackViewModel(TipoFeedback.Erro, _mensagem, new[] { html.ToString() }, _tipoAcaoOcultar);
                        }
                        else
                        {
                            feedbackViewModel = new FeedbackViewModel(TipoFeedback.Erro, _mensagem, new[] { _mensagemAdicional }, _tipoAcaoOcultar);
                        }

                        result = new ViewResult { ViewName = "Feedback" };

                        ((ViewResult)result).ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState);
                        ((ViewResult)result).ViewData.Model = feedbackViewModel;

                        break;
                }

                context.Result = result;

                base.OnException(context);
            }
        }
    }

}
