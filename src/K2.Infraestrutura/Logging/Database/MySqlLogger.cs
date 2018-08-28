using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;

namespace K2.Infraestrutura.Logging.Database
{
    public class MySqlLogger : ILogger
    {
        private readonly string _connectionString;
        private readonly string _nomeCategoria;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MySqlLogger(string nomeCategoria, string connectionString, IHttpContextAccessor httpContextAccessor)
        {
            _nomeCategoria = nomeCategoria;
            _connectionString = connectionString;
            _httpContextAccessor = httpContextAccessor;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            var mensagem = formatter(state, exception);

            if (string.IsNullOrEmpty(mensagem))
                return;

            if (exception != null)
                mensagem += " (" + exception.GetBaseException().Message + ")";

            using (var connection = new MySqlConnection(_connectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                using (var command = new MySqlCommand("INSERT INTO `log` (`NomeOrigem`, `Data`, `Tipo`, `Mensagem`, `Usuario`, `Stacktrace`) VALUES (@origem, @data, @tipo, @mensagem, @usuario, @stacktrace);", connection))
                {
                    command.Parameters.AddWithValue("origem", _nomeCategoria);
                    command.Parameters.AddWithValue("data", TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")));
                    command.Parameters.AddWithValue("tipo", logLevel.ToString());
                    command.Parameters.AddWithValue("mensagem", mensagem);
                    command.Parameters.AddWithValue("usuario", _httpContextAccessor.HttpContext.Request.Path);
                    command.Parameters.AddWithValue("stacktrace", exception != null ? exception.StackTrace : null);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
