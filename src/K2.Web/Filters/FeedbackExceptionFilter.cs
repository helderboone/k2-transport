using System;
using System.Text;
using K2.Web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace K2.Web.Filters
{
    public class FeedbackExceptionFilterAttribute : TypeFilterAttribute
    {
        public FeedbackExceptionFilterAttribute(string mensagem, TipoAcaoOcultarFeedback tipoAcaoOcultar, string mensagemAdicional = "", TipoFeedbackResponse tipoResponse = TipoFeedbackResponse.Html) 
            : base(typeof(FeedbackExceptionFilter))
        {
            Arguments = new object[] { mensagem, tipoAcaoOcultar, mensagemAdicional, tipoResponse };
        }

        private class FeedbackExceptionFilter : IExceptionFilter
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

            public void OnException(ExceptionContext context)
            {
                FeedbackViewModel feedback = null;
                IActionResult result = null;

                switch (_tipoResponse)
                {
                    case TipoFeedbackResponse.Json:

                        feedback = new FeedbackViewModel(TipoFeedback.ERRO, _mensagem, _mensagemAdicional, _tipoAcaoOcultar);

                        if (_hostingEnvironment.IsDevelopment())
                        {
                            result = new JsonResult(new {
                                feedback.Tipo,
                                feedback.Mensagem,
                                feedback.MensagemAdicional,
                                Exception = context.Exception.Message,
                                BaseException = context.Exception.GetBaseException().Message,
                                context.Exception.StackTrace
                            });
                        }
                        else
                        {
                            result = new JsonResult(new
                            {
                                feedback.Tipo,
                                feedback.Mensagem,
                                feedback.MensagemAdicional,
                                Exception = context.Exception.Message,
                                BaseException = context.Exception.GetBaseException().Message
                            });
                        }

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
                            html.Append($"<p>Base exception: {context.Exception.GetBaseException().Message}</p>");
                            html.Append($"<p>Stack trace: <br><div>{context.Exception.GetBaseException().Message}</div></p>");

                            feedback = new FeedbackViewModel(TipoFeedback.ERRO, _mensagem, html.ToString(), _tipoAcaoOcultar);
                        }
                        else
                        {
                            feedback = new FeedbackViewModel(TipoFeedback.ERRO, _mensagem, _mensagemAdicional, _tipoAcaoOcultar);
                        }

                        result = new ViewResult { ViewName = "Feedback" };

                        ((ViewResult)result).ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), context.ModelState);
                        ((ViewResult)result).ViewData.Model = feedback;

                        break;
                }

                context.Result = result;
            }
        }
    }

}
