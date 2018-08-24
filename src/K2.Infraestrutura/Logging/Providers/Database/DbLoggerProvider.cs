using Microsoft.Extensions.Logging;

namespace K2.Infraestrutura.Logging.Providers.Database
{
    public class DbLoggerProvider : ILoggerProvider
    {
        private readonly ILogRepositorio _logRepositorio;
        private readonly LogLevel _minLogLevel;

        public DbLoggerProvider(LogLevel minLogLevel, ILogRepositorio logRepositorio)
        {
            _minLogLevel    = minLogLevel;
            _logRepositorio = logRepositorio;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new DbLogger(categoryName, _minLogLevel, _logRepositorio);
        }

        public void Dispose()
        {
            
        }
    }
}
