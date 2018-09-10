using JNogueira.Infraestrutura.Utilzao;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de um cliente
    /// </summary>
    public class CadastrarClienteEntrada : CadastrarUsuarioEntrada
    {
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

        public CadastrarClienteEntrada(
            string nome,
            string email,
            string senha,
            string cpf,
            string rg,
            string celular,
            string cep = null,
            string endereco = null,
            string municipio = null,
            string uf = null)
            : base(nome, email, senha, cpf, rg, celular, TipoPerfil.Cliente)
        {
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
