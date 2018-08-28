using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace K2.Infraestrutura.Logging.Database
{
    public class MySqlLoggerProvider : ILoggerProvider
    {
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MySqlLoggerProvider(string connectionString, IHttpContextAccessor httpContextAccessor)
        {
            _connectionString = connectionString;
            _httpContextAccessor = httpContextAccessor;
        }

        public ILogger CreateLogger(string categoryName) => new MySqlLogger(categoryName, _connectionString, _httpContextAccessor);

        public void Dispose()
        {
            
        }
    }

    public static class MySqlLoggerProviderExtensions
    {
        public static ILoggerFactory AddMySqlLoggerProvider(this ILoggerFactory loggerFactory, string connectionString, IHttpContextAccessor httpContextAccessor)
        {
            loggerFactory.AddProvider(new MySqlLoggerProvider(connectionString, httpContextAccessor));
            return loggerFactory;
        }
    }
}
