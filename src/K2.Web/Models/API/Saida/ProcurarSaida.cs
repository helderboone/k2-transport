using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe para padronização das saídas da API relacionadas a procura
    /// </summary>
    public class ProcurarSaida : Saida<ProcurarRetorno>
    {
        public ProcurarSaida(bool sucesso, IEnumerable<string> mensagens, ProcurarRetorno retorno)
            : base(sucesso, mensagens, retorno)
        {

        }

        public IEnumerable<T> ObterRegistros<T>() => base.Retorno?.Registros.Select(x => x.ToObject<T>()).AsEnumerable();

        public static ProcurarSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ProcurarSaida>(json)
                : null;
        }
    }

    public class ProcurarRetorno
    {
        public int? PaginaIndex { get; set; }

        public int? PaginaTamanho { get; set; }

        public string OrdenarPor { get; set; }

        public string OrdenarSentido { get; set; }

        public int TotalRegistros { get; set; }

        public int? TotalPaginas { get; set; }

        public IEnumerable<JObject> Registros { get; set; }
    }
}
