using K2.Api.Filters;
using K2.Dominio;
using K2.Dominio.Interfaces.Dados;
using K2.Dominio.Interfaces.Infraestrutura;
using K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios;
using K2.Dominio.Interfaces.Servicos;
using K2.Dominio.Servicos;
using K2.Infraestrutura;
using K2.Infraestrutura.Dados;
using K2.Infraestrutura.Dados.Repositorios;
using K2.Infraestrutura.Logging.Database;
using K2.Infraestrutura.Logging.Slack;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Net;

namespace K2.Api
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
            // AddSingleton: instância configurada de forma que uma única referência das mesmas seja empregada durante todo o tempo em que a aplicação permanecer em execução
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IEmailHelper, SmtpHelper>(x => new SmtpHelper(Configuration["Smtp:Servidor"], Configuration["Smtp:Porta"], Configuration["Smtp:Usuario"], Configuration["Smtp:Senha"]));

            // Configuração realizada, seguindo o artigo "ASP.NET Core 2.0: autenticação em APIs utilizando JWT" 
            // (https://medium.com/@renato.groffe/asp-net-core-2-0-autentica%C3%A7%C3%A3o-em-apis-utilizando-jwt-json-web-tokens-4b1871efd)

            var tokenConfig = new JwtTokenConfig();

            // Extrai as informações do arquivo appsettings.json, criando um instância da classe "JwtTokenConfig"
            new ConfigureFromConfigurationOptions<JwtTokenConfig>(Configuration.GetSection("JwtTokenConfig"))
                .Configure(tokenConfig);

            services.AddSingleton(tokenConfig);

            services.AddScoped<EfDataContext, EfDataContext>(x => new EfDataContext(Configuration["K2ConnectionString"]));
            services.AddScoped<IUow, Uow>();

            // AddTransient: determina que referências desta classe sejam geradas toda vez que uma dependência for encontrada
            services.AddTransient<IUsuarioRepositorio, UsuarioRepositorio>();
            services.AddTransient<IClienteRepositorio, ClienteRepositorio>();
            services.AddTransient<IMotoristaRepositorio, MotoristaRepositorio>();
            services.AddTransient<IProprietarioCarroRepositorio, ProprietarioCarroRepositorio>();
            services.AddTransient<ICarroRepositorio, CarroRepositorio>();
            services.AddTransient<ILocalidadeRepositorio, LocalidadeRepositorio>();
            services.AddTransient<IViagemRepositorio, ViagemRepositorio>();
            services.AddTransient<IReservaRepositorio, ReservaRepositorio>();
            services.AddTransient<IReservaDependenteRepositorio, ReservaDependenteRepositorio>();

            services.AddTransient<IUsuarioServico, UsuarioServico>();
            services.AddTransient<IClienteServico, ClienteServico>();
            services.AddTransient<IMotoristaServico, MotoristaServico>();
            services.AddTransient<IProprietarioCarroServico, ProprietarioCarroServico>();
            services.AddTransient<ICarroServico, CarroServico>();
            services.AddTransient<ILocalidadeServico, LocalidadeServico>();
            services.AddTransient<IViagemServico, ViagemServico>();
            services.AddTransient<IReservaServico, ReservaServico>();

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
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Valida a assinatura de um token recebido
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        // Verifica se um token recebido ainda é válido
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        // Tempo de tolerância para a expiração de um token (utilizado
                        // caso haja problemas de sincronismo de horário entre diferentes
                        // computadores envolvidos no processo de comunicação)
                        ClockSkew = TimeSpan.Zero,

                        ValidIssuer      = tokenConfig.Issuer,
                        ValidAudience    = tokenConfig.Audience,
                        IssuerSigningKey = tokenConfig.Key
                    };
                });

            // AddAuthorization: ativará o uso de tokens com o intuito de autorizar ou não o acesso a recursos da aplicação
            services.AddAuthorization(options =>
            {
                // Adiciona as policies de acesso, definindo os claimns existentes em cada policy.
                options.AddPolicy(TipoPoliticaAcesso.Administrador, policy => policy.RequireClaim("Perfil", TipoPerfil.Administrador).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                options.AddPolicy(TipoPoliticaAcesso.ProprietarioCarro, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == "Perfil" && (c.Value == TipoPerfil.ProprietarioCarro || c.Value == TipoPerfil.Administrador))).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                options.AddPolicy(TipoPoliticaAcesso.Motorista, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == "Perfil" && (c.Value == TipoPerfil.Motorista || c.Value == TipoPerfil.Administrador))).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
                options.AddPolicy(TipoPoliticaAcesso.MotoristaOuProprietarioCarro, policy => policy.RequireAssertion(context => context.User.HasClaim(c => c.Type == "Perfil" && (c.Value == TipoPerfil.Motorista || c.Value == TipoPerfil.ProprietarioCarro || c.Value == TipoPerfil.Administrador))).AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme));
            });

            services
                .AddMvc(options => options.Filters.Add(typeof(CustomModelStateValidationFilter)))
                .AddJsonOptions(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            // Habilita a compressão do response
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json" });
            });

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory, IHttpContextAccessor httpContextAccessor)
        {
            loggerFactory
                // Adiciona o logger para gravar no banco de dados.
                .AddMySqlLoggerProvider(Configuration["K2ConnectionString"], httpContextAccessor)
                // Adiciona o logger para mandar mensagem pelo Slack.
                .AddSlackLoggerProvider(Configuration["Slack:Webhook"], Configuration["Slack:Channel"], httpContextAccessor, Configuration["Slack:UserName"]);

            var cultureInfo = new CultureInfo("pt-BR");

            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UsePathBase("/api");

            app.UseExceptionHandler($"/feedback/{(int)HttpStatusCode.InternalServerError}");

            // Customiza as páginas de erro
            app.UseStatusCodePagesWithReExecute("/feedback/{0}");

            // Utiliza a compressão do response
            app.UseResponseCompression();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
