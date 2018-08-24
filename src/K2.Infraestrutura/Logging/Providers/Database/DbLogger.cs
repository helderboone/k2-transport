using Microsoft.Extensions.Logging;
using System;

namespace K2.Infraestrutura.Logging.Providers.Database
{
    public class DbLogger : ILogger
    {
        private readonly string _nomeCategoria;
        private readonly LogLevel _minLogLevel;
        private readonly ILogRepositorio _logRepositorio;

        public DbLogger(string nomeCategoria, LogLevel minLogLevel, ILogRepositorio logRepositorio)
        {
            _nomeCategoria = nomeCategoria;
            _minLogLevel = minLogLevel;
            _logRepositorio = logRepositorio;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel >= _minLogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            var mensagem = formatter(state, exception);

            if (string.IsNullOrEmpty(mensagem))
                return;

            if (exception != null)
            {
                mensagem += "\n" + exception.ToString();
            }

            var registroLog = new RegistroLog
            {
                Tipo        = logLevel.ToString(),
                Mensagem    = mensagem,
                DataCriacao = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")),
                Stacktrace  = exception?.StackTrace
            };

            _logRepositorio.Inserir(registroLog);
        }
    }
}
