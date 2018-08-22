using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace K2.Web.Filters
{
    public class FeedbackExceptionFilterAttribute : TypeFilterAttribute
    {
        public FeedbackExceptionFilterAttribute(string mensagem, TipoAcaoAoOcultarFeedback tipoAcaoOcultar, string mensagemAdicional = "") 
            : base(typeof(FeedbackExceptionFilter))
        {
            Arguments = new object[] { mensagem, tipoAcaoOcultar, mensagemAdicional };
        }

        private class FeedbackExceptionFilter : ExceptionFilterAttribute
        {
            private readonly IHostingEnvironment _hostingEnvironment;

            private readonly string _mensagem;
            private readonly TipoAcaoAoOcultarFeedback _tipoAcaoOcultar;
            private readonly string _mensagemAdicional;
            

            public FeedbackExceptionFilter(IHostingEnvironment hostingEnvironment, string mensagem, TipoAcaoAoOcultarFeedback tipoAcaoOcultar, string mensagemAdicional)
            {
                _hostingEnvironment = hostingEnvironment;

                _mensagem = mensagem;
                _tipoAcaoOcultar = tipoAcaoOcultar;
                _mensagemAdicional = mensagemAdicional;
            }

            public override void OnException(ExceptionContext context)
            {
                HandleException(context);

                base.OnException(context);
            }

            private void HandleException(ExceptionContext context)
            {
                Feedback feedback;

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

                    feedback = new Feedback(TipoFeedback.Erro, _mensagem, new[] { html.ToString() }, _tipoAcaoOcultar);
                }
                else
                {
                    feedback = new Feedback(TipoFeedback.Erro, _mensagem, new[] { _mensagemAdicional }, _tipoAcaoOcultar);
                }

                var tipoResponse = context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest"
                    ? TipoFeedbackResponse.Json
                    : TipoFeedbackResponse.Html;

                context.Result = new FeedbackResult(feedback, tipoResponse);
            }
        }
    }
}