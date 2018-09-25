using K2.Infraestrutura.Logging.Database;
using K2.Infraestrutura.Logging.Slack;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
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
                options.AddPolicy(TipoPerfil.Administrador, policy => policy.RequireClaim("Perfil", TipoPerfil.Administrador).AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme));
                options.AddPolicy(TipoPerfil.ProprietarioCarro, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == "Perfil" && (c.Value == TipoPerfil.ProprietarioCarro || c.Value == TipoPerfil.Administrador))).AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme));
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
        {
            loggerFactory
                // Adiciona o logger para gravar no banco de dados.
                .AddMySqlLoggerProvider(Configuration["K2ConnectionString"], httpContextAccessor)
                // Adiciona o logger para mandar mensagem pelo Slack.
                .AddSlackLoggerProvider(Configuration["Slack:Webhook"], Configuration["Slack:Channel"], httpContextAccessor, Configuration["Slack:UserName"]);

            app.UseExceptionHandler($"/feedback/{(int)HttpStatusCode.InternalServerError}");

            // Customiza as páginas de erro
            app.UseStatusCodePagesWithReExecute("/feedback/{0}");

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
