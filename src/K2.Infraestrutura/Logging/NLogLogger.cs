using Microsoft.Extensions.Logging;
using System;

namespace K2.Infraestrutura.Logging
{
    public class NLogLogger : ILogger
    {
        private readonly NLog.Logger _nLogLogger;

        public NLogLogger(NLog.Logger nLogLogger)
        {
            _nLogLogger = nLogLogger;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            var message = formatter(state, exception);

            if (string.IsNullOrEmpty(message))
                return;

            //if (exception != null)
            //    message += "\n" + exception.ToString();

            var nLogEvent = new NLog.LogEventInfo()
            {
                Level = NLog.LogLevel.FromOrdinal((int)logLevel),
                Message = message,
                Exception = exception,
                TimeStamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time"))
            };

            _nLogLogger.Log(nLogEvent);
        }
    }
}
