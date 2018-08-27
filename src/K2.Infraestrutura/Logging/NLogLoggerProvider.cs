using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Web;

namespace K2.Infraestrutura.Logging
{
    public class NLogLoggerProvider : ILoggerProvider
    {
        public LogLevel AplicacaoMinLevel { get; }

        public LogLevel MicrosoftMinLevel { get; }

        public LoggingConfiguration Configuracao { get; }

        private static NLog.Logger _nLogger;

        public NLogLoggerProvider(LogLevel aplicacaoMinLevel, LogLevel microsoftMinLevel, string connectionString)
        {
            AplicacaoMinLevel = aplicacaoMinLevel;
            MicrosoftMinLevel = microsoftMinLevel;

            Configuracao = new LoggingConfiguration();

            var dbTarget = new DatabaseTarget("mySql")
            {
                DBProvider = "MySql.Data.MySqlClient.MySqlConnection, MySql.Data",
                ConnectionString = connectionString,
                CommandText = "INSERT INTO `log` (`NomeOrigem`, `Data`, `Tipo`, `Mensagem`, `Usuario`, `Stacktrace`) VALUES (@origem, @data, @tipo, @mensagem, @usuario, @stacktrace);",
            };

            dbTarget.Parameters.Add(new DatabaseParameterInfo("@origem", new SimpleLayout("${logger}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@data", new SimpleLayout("${longdate}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@tipo", new SimpleLayout("${uppercase:${level}}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@mensagem", new SimpleLayout("${message}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@usuario", new SimpleLayout("${aspnet-user-identity}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@stacktrace", new SimpleLayout("${exception:tostring}")));

            Configuracao.AddTarget(dbTarget);

            Configuracao.LoggingRules.Add(new LoggingRule("Microsoft.*", NLog.LogLevel.FromOrdinal((int)MicrosoftMinLevel), dbTarget));
            Configuracao.LoggingRules.Add(new LoggingRule("K2.*", NLog.LogLevel.FromOrdinal((int)AplicacaoMinLevel), dbTarget));

            NLog.Common.InternalLogger.LogFile = @"c:\temp\internal-nlog2.txt";
            NLog.Common.InternalLogger.LogLevel = NLog.LogLevel.Trace;

            _nLogger = NLogBuilder.ConfigureNLog(Configuracao).GetCurrentClassLogger();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new NLogLogger(_nLogger);
        }

        public void Dispose()
        {
            NLog.LogManager.Shutdown();
        }
    }

    public static class NLogProviderExtensions
    {
        public static ILoggerFactory AdicionarNLog(this ILoggerFactory factory, LogLevel aplicacaoMinLevel, LogLevel microsoftMinLevel, string connectionString)
        {
            factory.AddProvider(new NLogLoggerProvider(aplicacaoMinLevel, microsoftMinLevel, connectionString));

            return factory;
        }

        public static NLogLoggerProvider AdicionarMySqlTarget(this NLogLoggerProvider provider, string connectionString)
        {
            var dbTarget = new DatabaseTarget("mySql")
            {
                DBProvider = "MySql.Data.MySqlClient.MySqlConnection, MySql.Data",
                ConnectionString = connectionString,
                CommandText = "INSERT INTO `log` (`NomeOrigem`, `Data`, `Tipo`, `Mensagem`, `Usuario`, `Stacktrace`) VALUES (@origem, @data, @tipo, @mensagem, @usuario, @stacktrace);",
            };

            dbTarget.Parameters.Add(new DatabaseParameterInfo("@origem", new SimpleLayout("${logger}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@data", new SimpleLayout("${longdate}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@tipo", new SimpleLayout("${uppercase:${level}}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@mensagem", new SimpleLayout("${message}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@usuario", new SimpleLayout("${aspnet-user-identity}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@stacktrace", new SimpleLayout("${exception:tostring}")));

            provider.Configuracao.AddTarget(dbTarget);

            provider.Configuracao.LoggingRules.Add(new LoggingRule("Microsoft.*", NLog.LogLevel.FromOrdinal((int)provider.MicrosoftMinLevel), dbTarget));
            provider.Configuracao.LoggingRules.Add(new LoggingRule("K2.*", NLog.LogLevel.FromOrdinal((int)provider.AplicacaoMinLevel), dbTarget));

            return provider;
        }
    }
}
