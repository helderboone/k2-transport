using JNogueira.Infraestrutura.Utilzao.Slack;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace K2.Infraestrutura.Logging.Slack
{
    public class SlackLogger : ILogger
    {
        private readonly string _webHookUrl;
        private readonly string _nomeOrigem;
        private readonly string _channel;
        private readonly string _userName;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly SlackUtil _slackUtil;
        
        public SlackLogger(string nomeOrigem, string webHookUrl, string channel, IHttpContextAccessor httpContextAccessor, string userName = null)
        {
            _webHookUrl = webHookUrl;
            _nomeOrigem = nomeOrigem;
            _channel = channel;
            _userName = userName;

            _slackUtil = new SlackUtil(webHookUrl);
            _httpContextAccessor = httpContextAccessor;
            
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            var mensagem = formatter(state, exception);

            if (string.IsNullOrEmpty(mensagem))
                return;

            var slackMensagem = new SlackMensagem(_channel, mensagem, _userName);

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.Information:
                    slackMensagem.Titulo = logLevel == LogLevel.Information ? "Info" : logLevel.ToString();
                    slackMensagem.DefinirTipo(TipoSlackMensagem.Info);
                    break;
                case LogLevel.Warning:
                    slackMensagem.Titulo = "Atenção";
                    slackMensagem.DefinirTipo(TipoSlackMensagem.Aviso);
                    break;
                case LogLevel.Error:
                    slackMensagem.Titulo = "Erro";
                    slackMensagem.DefinirTipo(TipoSlackMensagem.Erro);
                    break;
                case LogLevel.Critical:
                    slackMensagem.Titulo = logLevel.ToString();
                    slackMensagem.DefinirTipo(TipoSlackMensagem.Aviso);
                    break;
                case LogLevel.None:
                    slackMensagem.DefinirTipo(TipoSlackMensagem.Info);
                    break;
            }

            var slackInfoAdicionais = new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("Origem", _nomeOrigem) };

            if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext?.User?.Identity?.Name))
                slackInfoAdicionais.Add(new KeyValuePair<string, string>("Usuário", _httpContextAccessor.HttpContext.User.Identity.Name));

            if (exception != null)
            {
                var logExcpetion = new LogException(exception, _httpContextAccessor);

                if (logExcpetion.Request != null)
                    slackInfoAdicionais.Add(new KeyValuePair<string, string>("Rotal", logExcpetion.Request.Rota));

                _slackUtil.Postar(slackMensagem, exception, slackInfoAdicionais);
            }
            else
            {
                _slackUtil.Postar(slackMensagem, infoAdicionais: slackInfoAdicionais);
            }
        }
    }
}