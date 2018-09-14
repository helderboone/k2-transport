using JNogueira.Infraestrutura.NotifiqueMe;
using JNogueira.Infraestrutura.Utilzao;
using K2.Dominio.Entidades;
using System.Reflection;

namespace K2.Dominio.Comandos.Entrada
{
    public class ProcurarClienteEntrada : ProcurarEntrada
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }

        public ProcurarClienteEntrada(string ordenarPor,
            string ordenarSentido,
            int? paginaIndex = null,
            int? paginaTamanho = null)
            : base(
                string.IsNullOrEmpty(ordenarPor) ? "Id" : ordenarPor,
                string.IsNullOrEmpty(ordenarSentido) ? "ASC" : ordenarSentido,
                paginaIndex,
                paginaTamanho)
        {
            this.Cpf = this.Cpf?.RemoverCaracter(".", "-");
            this.Rg = this.Rg?.ToUpper().RemoverCaracter(".", "-", "/");

            this.Validar();
        }

        private void Validar()
        {
            this.NotificarSeNulo(typeof(Cliente).GetProperty(this.OrdenarPor, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance), $"A propriedade {this.OrdenarPor} não pertence a classe \"Cliente\".");
        }
    }
}
