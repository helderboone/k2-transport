using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Interfaces.Comandos;
using System;

namespace K2.Dominio.Comandos.Entrada
{
    public abstract class ProcurarEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Página atual da listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaIndex { get; private set; }

        /// <summary>
        /// Quantidade de registros exibidos por página na listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaTamanho { get; private set; }

        /// <summary>
        /// Nome da propriedade que deverá ser utilizada para ordenação do resultado da pesquisa
        /// </summary>
        public string OrdenarPor { get; private set; }

        /// <summary>
        /// Sentido da ordenação do resultado da pesquisa: "ASC" para crescente / "DESC" para decrescente
        /// </summary>
        public string OrdenarSentido { get; private set; }

        public ProcurarEntrada(string ordenarPor, string ordenarSentido, int? paginaIndex = null, int? paginaTamanho = null)
        {
            this.OrdenarPor = ordenarPor;
            this.OrdenarSentido = !string.Equals(ordenarSentido, "ASC", StringComparison.InvariantCultureIgnoreCase) && !string.Equals(ordenarSentido, "DESC", StringComparison.InvariantCultureIgnoreCase)
                ? "ASC"
                : ordenarSentido;
            this.PaginaIndex = paginaIndex;
            this.PaginaTamanho = paginaTamanho;

            this.Validar();
        }

        public bool Paginar()
        {
            return this.PaginaIndex.HasValue && this.PaginaTamanho.HasValue;
        }

        private void Validar()
        {
            if (this.PaginaIndex.HasValue)
                this.NotificarSeMenorQue(this.PaginaIndex.Value, 1, "Index da paginação é inválido.");

            if (this.PaginaTamanho.HasValue)
                this.NotificarSeMenorQue(this.PaginaTamanho.Value, 1, "Tamanho da página utilizado na paginação é inválido.");
        }
    }
}
