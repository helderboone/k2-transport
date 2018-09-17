using Newtonsoft.Json;

namespace K2.Web.Models
{
    public abstract class BaseModel
    {
        public string ObterJson() => this == null ? string.Empty : JsonConvert.SerializeObject(this);
    }
}
