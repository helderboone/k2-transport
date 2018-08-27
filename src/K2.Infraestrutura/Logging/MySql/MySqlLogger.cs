using Microsoft.Extensions.Logging;
using System;
using MySqlClient = MySql.Data.MySqlClient;

namespace K2.Infraestrutura.Logging.MySql
{
    public class MySqlLogger : ILogger
    {
        private readonly string _connectionString;
        private readonly string _nomeCategoria;
        private readonly Func<string, LogLevel, bool> _filtro;

        public MySqlLogger(string nomeCategoria, string connectionString, Func<string, LogLevel, bool> filtro)
        {
            _nomeCategoria = nomeCategoria;
            _connectionString = connectionString;
            _filtro = filtro;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => _filtro == null || _filtro(_nomeCategoria, logLevel);

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
                mensagem += " (" + exception.GetBaseException().Message + ")";

            using (var connection = new MySqlClient.MySqlConnection(_connectionString))
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                using (var command = new MySqlClient.MySqlCommand("INSERT INTO `log` (`NomeOrigem`, `Data`, `Tipo`, `Mensagem`, `Usuario`, `Stacktrace`) VALUES (@origem, @data, @tipo, @mensagem, @usuario, @stacktrace);", connection))
                {
                    command.Parameters.AddWithValue("origem", _nomeCategoria);
                    command.Parameters.AddWithValue("data", TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")));
                    command.Parameters.AddWithValue("tipo", logLevel.ToString());
                    command.Parameters.AddWithValue("mensagem", mensagem);
                    command.Parameters.AddWithValue("usuario", null);
                    command.Parameters.AddWithValue("stacktrace", exception != null ? exception.StackTrace : null);

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
