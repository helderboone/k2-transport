using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace K2.Infraestrutura.Logging
{
    public class LogException
    {
        public string Rota { get; }

        public Dictionary<string, string> Headers { get; }

        public LogExceptionInfo ExceptionInfo { get; }

        public LogException(Exception exception, IHttpContextAccessor httpContextAccessor)
        {
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            if (httpContextAccessor == null)
                throw new ArgumentNullException(nameof(httpContextAccessor));

            var request = httpContextAccessor.HttpContext?.Request;

            if (request != null)
            {
                var uriBuilder = new UriBuilder
                {
                    Scheme = request.Scheme,
                    Host = request.Host.Host,
                    Path = request.Path.ToString(),
                    Query = request.QueryString.ToString()
                };

                if (request.Host.Port.HasValue && request.Host.Port.Value != 80)
                    uriBuilder.Port = request.Host.Port.Value;

                this.Rota = uriBuilder.Uri.ToString();

                foreach (var item in request.Headers.Where(x => x.Value.Any()))
                    this.Headers.Add(item.Key, string.Join(",", item.Value.ToArray()));
            }

            this.ExceptionInfo = new LogExceptionInfo(exception);
        }
    }

    //public class LogRequest
    //{


    //    public LogRequest(IHttpContextAccessor httpContextAccessor)
    //    {
    //        httpContextAccessor.HttpContext.Request.Headers.
    //    }
    //}

    public class LogExceptionInfo
    {
        public string ExceptionMensagem { get; }

        public string BaseExceptionMensagem { get; }

        public string StackTrace { get; }

        public string Source { get; }

        public LogExceptionInfo(Exception exception)
        {
            this.ExceptionMensagem = exception.Message;

            this.BaseExceptionMensagem = exception.GetBaseException().Message;

            this.StackTrace = exception.StackTrace;

            this.Source = exception.Source;
        }
    }
}
