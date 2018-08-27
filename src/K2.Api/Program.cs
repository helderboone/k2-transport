using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace K2.Api
{
    public class Program
    {
        public static IConfiguration _configuration;

        public static void Main(string[] args)
        {
            CreateWebHost(args).Run();
        }

        public static IWebHost CreateWebHost(string[] args) =>
            WebHost
                .CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
