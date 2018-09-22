using JNogueira.Infraestrutura.NotifiqueMe;
using JNogueira.Infraestrutura.Utilzao;
using K2.Dominio.Entidades;
using System.Reflection;

namespace K2.Dominio.Comandos.Entrada
{
    public class ProcurarProprietarioCarroEntrada : ProcurarEntrada
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        private string _cpf;

        public string Cpf
        {
            get => _cpf?.RemoverCaracter(".", "-");
            set => _cpf = value;
        }

        private string _rg;

        public string Rg
        {
            get => _rg?.RemoverCaracter(".", "-", "/");
            set => _rg = value;
        }

        public ProcurarProprietarioCarroEntrada(string ordenarPor,
            string ordenarSentido,
            int? paginaIndex = null,
            int? paginaTamanho = null)
            : base(
                string.IsNullOrEmpty(ordenarPor) ? "Nome" : ordenarPor,
                string.IsNullOrEmpty(ordenarSentido) ? "ASC" : ordenarSentido,
                paginaIndex,
                paginaTamanho)
        {
            this.Validar();
        }

        private void Validar()
        {
            this.NotificarSeNulo(typeof(ProprietarioCarro).GetProperty(this.OrdenarPor, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance), $"A propriedade {this.OrdenarPor} não pertence a classe \"ProprietarioCarro\".");
        }
    }
}
