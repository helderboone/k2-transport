using Microsoft.Extensions.Logging;
using System;

namespace K2.Infraestrutura.Logging.MySql
{
    public class MySqlLoggerProvider : ILoggerProvider
    {
        private readonly string _connectionString;
        private readonly Func<string, LogLevel, bool> _filtro;

        public MySqlLoggerProvider(string connectionString, Func<string, LogLevel, bool> filtro)
        {
            _connectionString = connectionString;
            _filtro = filtro;
        }

        public ILogger CreateLogger(string categoryName) => new MySqlLogger(categoryName, _connectionString, _filtro);

        public void Dispose()
        {
            
        }
    }

    public static class MySqlLoggerExtensions
    {
        public static ILoggerFactory AddMySql(this ILoggerFactory factory, string connectionString, Func<string, LogLevel, bool> filtro = null)
        {
            factory.AddProvider(new MySqlLoggerProvider(connectionString, filtro));
            return factory;
        }
    }
}
