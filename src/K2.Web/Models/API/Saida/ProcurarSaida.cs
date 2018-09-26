using Newtonsoft.Json;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe para padronização das saídas da API relacionadas a procura
    /// </summary>
    public class ProcurarSaida : Saida
    {
        public ProcurarSaida(bool sucesso, IEnumerable<string> mensagens, ProcurarRetorno retorno)
            : base(sucesso, mensagens, retorno)
        {

        }

        public ProcurarRetorno ObterRetorno() => (ProcurarRetorno)this.Retorno;

        public new static ProcurarSaida Obter(string json)
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

        public IEnumerable<object> Registros { get; set; }
    }
}
