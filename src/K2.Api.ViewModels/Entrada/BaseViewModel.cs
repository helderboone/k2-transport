using Newtonsoft.Json;

namespace K2.Api.ViewModels
{
    public abstract class BaseViewModel
    {
        public string ObterJson() => this == null ? string.Empty : JsonConvert.SerializeObject(this);
    }
}
