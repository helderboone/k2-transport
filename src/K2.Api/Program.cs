using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Common;
using NLog.Config;
using NLog.Targets;
using NLog.Web;
using System;
using System.IO;

namespace K2.Api
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

            ConfigurarNLog();

            try
            {
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao subir a API.");
                throw;
            }
            finally
            {
                LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .UseNLog();

        private static void ConfigurarNLog()
        {
            var config = new LoggingConfiguration();

            InternalLogger.LogFile = @"c:\temp\internal-nlog.txt";
            InternalLogger.LogLevel = NLog.LogLevel.Trace;

            var dbTarget = new DatabaseTarget("mySql")
            {
                DBProvider       = "MySql.Data.MySqlClient.MySqlConnection, MySql.Data",
                ConnectionString = _configuration["K2ConnectionString"],
                CommandText      = "INSERT INTO `log` (`NomeOrigem`, `Data`, `Tipo`, `Mensagem`, `Usuario`, `Stacktrace`) VALUES (@origem, @data, @tipo, @mensagem, @usuario, @stacktrace);",
            };

            dbTarget.Parameters.Add(new DatabaseParameterInfo("@origem", new NLog.Layouts.SimpleLayout("${logger}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@data", new NLog.Layouts.SimpleLayout("${longdate}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@tipo", new NLog.Layouts.SimpleLayout("${uppercase:${level}}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@mensagem", new NLog.Layouts.SimpleLayout("${message}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@usuario", new NLog.Layouts.SimpleLayout("${aspnet-user-identity}")));
            dbTarget.Parameters.Add(new DatabaseParameterInfo("@stacktrace", new NLog.Layouts.SimpleLayout("${exception:tostring}")));

            config.AddTarget(dbTarget);

            config.LoggingRules.Add(new LoggingRule("Microsoft.*", NLog.LogLevel.Error, dbTarget));
            config.LoggingRules.Add(new LoggingRule("K2.*", NLog.LogLevel.Info, dbTarget));

            LogManager.Configuration = config;

            _logger = LogManager.GetCurrentClassLogger();
        }
    }
}
