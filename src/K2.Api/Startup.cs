using K2.Api.Middlewares;
using K2.Dominio;
using K2.Dominio.Interfaces.Dados;
using K2.Dominio.Interfaces.Dados.Repositorios;
using K2.Dominio.Interfaces.Servicos;
using K2.Dominio.Servicos;
using K2.Infraestrutura.Dados;
using K2.Infraestrutura.Dados.Repositorios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace K2.Api
{
    public class Startup
    {
        public static IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            services.AddScoped<EfDataContext, EfDataContext>(x => new EfDataContext(Configuration["K2ConnectionString"]));
            services.AddScoped<IUow, Uow>();

            // AddTransient: determina que referências desta classe sejam geradas toda vez que uma dependência for encontrada
            services.AddTransient<IUsuarioRepositorio, UsuarioRepositorio>();

            services.AddTransient<IUsuarioServico, UsuarioServico>();

            // Configuração realizada, seguindo o artigo "ASP.NET Core 2.0: autenticação em APIs utilizando JWT" 
            // (https://medium.com/@renato.groffe/asp-net-core-2-0-autentica%C3%A7%C3%A3o-em-apis-utilizando-jwt-json-web-tokens-4b1871efd)

            var tokenConfig = new JwtTokenConfig();

            // Extrai as informações do arquivo appsettings.json, criando um instância da classe "JwtTokenConfig"
            new ConfigureFromConfigurationOptions<JwtTokenConfig>(Configuration.GetSection("JwtTokenConfig"))
                .Configure(tokenConfig);

            // AddSingleton: instância configurada de forma que uma única referência das mesmas seja empregada durante todo o tempo em que a aplicação permanecer em execução
            services.AddSingleton(tokenConfig);

            services
                // AddAuthentication: especificará os schemas utilizados para a autenticação do tipo Bearer
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                // AddJwtBearer: definidas configurações como a chave e o algoritmo de criptografia utilizados, a necessidade de analisar se um token ainda é válido e o tempo de tolerância para expiração de um token
                .AddJwtBearer(options =>
                {
                    var paramsValidation = options.TokenValidationParameters;
                    paramsValidation.IssuerSigningKey = tokenConfig.Key;
                    paramsValidation.ValidAudience = tokenConfig.Audience;
                    paramsValidation.ValidIssuer = tokenConfig.Issuer;

                    // Valida a assinatura de um token recebido
                    paramsValidation.ValidateIssuerSigningKey = true;

                    // Verifica se um token recebido ainda é válido
                    paramsValidation.ValidateLifetime = true;

                    // Tempo de tolerância para a expiração de um token (utilizado
                    // caso haja problemas de sincronismo de horário entre diferentes
                    // computadores envolvidos no processo de comunicação)
                    paramsValidation.ClockSkew = TimeSpan.Zero;
                });

            // AddAuthorization: ativará o uso de tokens com o intuito de autorizar ou não o acesso a recursos da aplicação
            services.AddAuthorization(options =>
            {
                // Adiciona as policies de acesso, definindo os claimns existentes em cada policy.
                options.AddPolicy(PermissaoAcesso.Usuarios, policy => policy.RequireClaim(PermissaoAcesso.Usuarios).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
            });

            services
                //.AddMvc(options => options.Filters.Add(typeof(CustomModelStateValidationFilter)))
                .AddMvc()
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            // Habilita a compressão do response
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Entende que a página default é a "index.html" dentro da pasta "wwwroot"
            app.UseDefaultFiles();

            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Middleware customizado para interceptar erros HTTP e exceptions não tratadas
                app.UseCustomExceptionHandler();
            }

            // Utiliza a compressão do response
            app.UseResponseCompression();

            app.UseMvc();
        }
    }
}
