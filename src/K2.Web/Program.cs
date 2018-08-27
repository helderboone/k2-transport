using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Web;
using System;
using System.IO;
using System.Reflection;

namespace K2.Web
{
    public class Program
    {
        public static IConfiguration _configuration;

        private static Logger _logger;

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();

            // Configuração do NLog
            var nLogConfig = new LoggingConfiguration();

            InternalLogger.LogFile = @"c:\temp\internal-nlog.txt";
            InternalLogger.LogLevel = LogLevel.Trace;

            var dbTarget = new DatabaseTarget("mySql")
            {
                DBProvider = "MySql.Data.MySqlClient.MySqlConnection, MySql.Data",
                ConnectionString = _configuration["K2ConnectionString"],
                CommandText = "INSERT INTO `log` (`NomeOrigem`, `Data`, `Tipo`, `Mensagem`, `Usuario`, `Stacktrace`) VALUES (@origem, @data, @tipo, @mensagem, @usuario, @stacktrace);",
            };

            dbTarget.Parameters.Add(new DatabaseParameterInfo("@origem", new SimpleLayout("${logger}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@data", new SimpleLayout("${longdate}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@tipo", new SimpleLayout("${uppercase:${level}}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@mensagem", new SimpleLayout("${message}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@usuario", new SimpleLayout("${aspnet-user-identity}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@stacktrace", new SimpleLayout("${exception:tostring}")));

            nLogConfig.AddTarget(dbTarget);

            nLogConfig.LoggingRules.Add(new LoggingRule("Microsoft.*", LogLevel.Error, dbTarget));
            nLogConfig.LoggingRules.Add(new LoggingRule("K2.*", LogLevel.Info, dbTarget));

            //LogManager.Configuration = nLogConfig;

            _logger = NLogBuilder.ConfigureNLog(nLogConfig).GetCurrentClassLogger();

            try
            {
                CreateWebHostBuilder(args).Run();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao subir a aplicação.");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IWebHost CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseNLog()
            .Build();
    }
}
