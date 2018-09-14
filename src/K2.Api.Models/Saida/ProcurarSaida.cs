using Newtonsoft.Json;
using System.Collections.Generic;

namespace K2.Api.Models
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
        public int? PaginaIndex { get; }

        public int? PaginaTamanho { get; }

        public string OrdenarPor { get; }

        public string OrdenarSentido { get; }

        public int TotalRegistros { get; }

        public int? TotalPaginas { get; }

        public IEnumerable<object> Registros { get; }

        public ProcurarRetorno(int? paginaIndex, int? paginaTamanho, string ordenarPor, string ordenarSentido, int totalRegistros, int? totalPaginas, IEnumerable<object> registros)
        {
            PaginaIndex    = paginaIndex;
            PaginaTamanho  = paginaTamanho;
            OrdenarPor     = ordenarPor;
            OrdenarSentido = ordenarSentido;
            TotalRegistros = totalRegistros;
            TotalPaginas   = totalPaginas;
            Registros      = registros;
        }
    }
}
