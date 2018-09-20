using JNogueira.Infraestrutura.Utilzao;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o alterar um cliente
    /// </summary>
    public class AlterarClienteEntrada : AlterarUsuarioEntrada
    {
        /// <summary>
        /// ID do cliente
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// CEP do cliente
        /// </summary>
        public string Cep { get; }

        /// <summary>
        /// Endereço do cliente
        /// </summary>
        public string Endereco { get; }

        /// <summary>
        /// Município do cliente
        /// </summary>
        public string Municipio { get; }

        /// <summary>
        /// UF do cliente
        /// </summary>
        public string Uf { get; }

        public AlterarClienteEntrada(
            int id,
            string nome,
            string email,
            string cpf,
            string rg,
            string celular,
            bool ativo,
            string cep = null,
            string endereco = null,
            string municipio = null,
            string uf = null)
            : base(id, nome, email, cpf, rg, celular, ativo, false)
        {
            Id        = id;
            Cep       = cep?.RemoverCaracter(".", "-", "/");
            Endereco  = endereco?.ToUpper();
            Municipio = municipio?.ToUpper();
            Uf        = uf?.ToUpper();
        }

        public override string ToString()
        {
            return this.Nome.ToUpper();
        }
    }
}
