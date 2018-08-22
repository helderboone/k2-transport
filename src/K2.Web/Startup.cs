using K2.Dominio;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                options.AddPolicy(Perfil.Administrador, policy => policy.RequireClaim(Perfil.Administrador).AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme));
                options.AddPolicy("pepeca", policy => policy.RequireClaim("pepeca").AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme));
            });

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler($"/feedback/{(int)HttpStatusCode.InternalServerError}");
            }

            // Customiza as páginas de erro
            app.UseStatusCodePagesWithReExecute("/feedback/{0}");

            app.UseAuthentication();

            app.UseStaticFiles();

            app.UseMvc();
        }
    }
}
