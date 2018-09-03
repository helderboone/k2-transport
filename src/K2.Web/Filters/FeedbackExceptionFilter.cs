using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

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
            private readonly ILoggerFactory _loggerFactory;

            private readonly string _mensagem;
            private readonly TipoAcaoAoOcultarFeedback _tipoAcaoOcultar;
            private readonly string _mensagemAdicional;

            public FeedbackExceptionFilter(IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory, string mensagem, TipoAcaoAoOcultarFeedback tipoAcaoOcultar, string mensagemAdicional)
            {
                _hostingEnvironment = hostingEnvironment;

                _mensagem = mensagem;
                _tipoAcaoOcultar = tipoAcaoOcultar;
                _mensagemAdicional = mensagemAdicional;
                _loggerFactory = loggerFactory;
            }

            public override void OnException(ExceptionContext context)
            {
                HandleException(context);

                base.OnException(context);
            }

            private void HandleException(ExceptionContext context)
            {
                var feedback = new Feedback(TipoFeedback.Erro, _mensagem, new[] { _mensagemAdicional  }, _tipoAcaoOcultar);

                var logger = _loggerFactory.CreateLogger<FeedbackExceptionFilter>();

                logger.LogError(context.Exception, _mensagem);

                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                context.Result = new FeedbackResult(feedback);
            }
        }
    }
}