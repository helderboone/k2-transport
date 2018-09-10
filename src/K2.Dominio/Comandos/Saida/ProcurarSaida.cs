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
            : base(true, new[] { "Procura realizada com sucesso." }, new
            {
                PaginaIndex = paginaIndex,
                PaginaTamanho = paginaTamanho,
                OrdenarPor = ordenarPor,
                OrdenarSentido = ordenarSentido,
                TotalRegistros = totalRegistros,
                TotalPaginas = totalPaginas,
                Registros = registros
            })
        {

        }

        public ProcurarSaida(IEnumerable<object> registros)
            : base(true, new[] { "Procura realizada com sucesso." }, new
            {
                TotalRegistros = registros != null ? registros.Count() : 0,
                Registros = registros
            })
        {

        }
    }
}
