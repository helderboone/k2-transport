using System.Collections.Generic;
using System.Linq;

namespace K2.Dominio.Comandos.Saida
{
    public class ProcurarSaida : Saida
    {
        public ProcurarSaida(
            IEnumerable<object> registros,
            string ordenarPor,
            string ordenarSentido,
            int totalRegistros,
            int? totalPaginas = null,
            int? paginaIndex = null,
            int? paginaTamanho = null)
            : base(true, new[] { "Procura realizada com sucesso." }, new ProcurarRetorno(
                paginaIndex,
                paginaTamanho,
                ordenarPor,
                ordenarSentido,
                totalRegistros,
                totalPaginas,
                registros))
        {

        }

        public ProcurarSaida(IEnumerable<object> registros)
            : base(true, new[] { "Procura realizada com sucesso." }, new ProcurarRetorno(
                null,
                null,
                null,
                null,
                registros != null ? registros.Count() : 0,
                null,
                registros))
        {

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
