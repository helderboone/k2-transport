using K2.Infraestrutura.Logging.Database;
using K2.Infraestrutura.Logging.Slack;
using K2.Web.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;
using System;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace K2.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddTransient<RestSharpHelper, RestSharpHelper>();
            services.AddTransient<DatatablesHelper, DatatablesHelper>();
            services.AddTransient<CookieHelper, CookieHelper>();
            services.AddTransient<CustomHtmlHelper, CustomHtmlHelper>();

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.LoginPath         = "/login";
                        options.AccessDeniedPath  = $"/feedback/{(int)HttpStatusCode.Forbidden}";
                        options.SlidingExpiration = true;
                        options.Cookie.Name       = "K2";
                        options.ExpireTimeSpan    = TimeSpan.FromHours(2); // Se o cookie não for persistente, a sessão ficará ativa por 2 horas
                    });

            services.AddAuthorization(options =>
            {
                options.AddPolicy(TipoPoliticaAcesso.Administrador, policy => policy.RequireClaim("Perfil", TipoPerfil.Administrador).AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme));
                options.AddPolicy(TipoPoliticaAcesso.ProprietarioCarro, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == "Perfil" && (c.Value == TipoPerfil.ProprietarioCarro || c.Value == TipoPerfil.Administrador))).AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme));
                options.AddPolicy(TipoPoliticaAcesso.Motorista, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == "Perfil" && (c.Value == TipoPerfil.Motorista || c.Value == TipoPerfil.Administrador))).AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme));
                options.AddPolicy(TipoPoliticaAcesso.AnalistaTI, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == "Perfil" && c.Value == TipoPerfil.Administrador) && context.User.HasClaim(c => c.Type == "Cpf" && c.Value == "09937454743")).AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme));
            });

            services.AddMvc();

            // Habilita a compressão do response
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json", "text/css", "text/html", "text/plain" });
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env)
        {
            loggerFactory
                // Adiciona o logger para gravar no banco de dados.
                .AddMySqlLoggerProvider(Configuration["K2ConnectionString"], httpContextAccessor)
                // Adiciona o logger para mandar mensagem pelo Slack.
                .AddSlackLoggerProvider(Configuration["Slack:Webhook"], Configuration["Slack:Channel"], httpContextAccessor, Configuration["Slack:UserName"]);

            var cultureInfo = new CultureInfo("pt-BR");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseExceptionHandler($"/feedback/{(int)HttpStatusCode.InternalServerError}");

            // Customiza as páginas de erro
            app.UseStatusCodePagesWithReExecute("/feedback/{0}");

            // Utiliza a compressão do response
            app.UseResponseCompression();

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc();

            RotativaConfiguration.Setup(env);
        }
    }
}
