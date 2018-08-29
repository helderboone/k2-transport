using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace K2.Infraestrutura.Logging.Slack
{
    public class SlackLoggerProvider : ILoggerProvider
    {
        private readonly string _webHookUrl;
        private readonly string _channel;
        private readonly string _userName;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public SlackLoggerProvider(string webHookUrl, string channel, IHttpContextAccessor httpContextAccessor, string userName = null)
        {
            _channel = channel;
            _userName = userName;
            _httpContextAccessor = httpContextAccessor;
            _webHookUrl = webHookUrl;
        }

        public ILogger CreateLogger(string categoryName) => new SlackLogger(categoryName, _webHookUrl, _channel, _httpContextAccessor, _userName);

        public void Dispose()
        {

        }
    }

    public static class SlackLoggerProviderExtensions
    {
        public static ILoggerFactory AddSlackLoggerProvider(this ILoggerFactory loggerFactory, string webHookUrl, string channel, IHttpContextAccessor httpContextAccessor, string userName = null)
        {
            loggerFactory.AddProvider(new SlackLoggerProvider(webHookUrl, channel, httpContextAccessor, userName));
            return loggerFactory;
        }
    }
}
