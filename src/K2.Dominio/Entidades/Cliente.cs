using K2.Dominio.Comandos.Entrada;

namespace K2.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um cliente
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// ID do cliente
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// CEP do cliente
        /// </summary>
        public string Cep { get; private set; }

        /// <summary>
        /// Descrição do endereço do cliente
        /// </summary>
        public string Endereco { get; private set; }

        /// <summary>
        /// Nome do município do cliente
        /// </summary>
        public string Municipio{ get; private set; }

        /// <summary>
        /// Sigla da UF do cliente
        /// </summary>
        public string Uf { get; private set; }

        /// <summary>
        /// Usuário associado ao cliente
        /// </summary>
        public Usuario Usuario { get; private set; }

        private Cliente()
        {

        }

        public Cliente(CadastrarClienteEntrada cadastrarEntrada)
        {
            if (cadastrarEntrada.Invalido)
                return;

            this.Usuario = new Usuario(cadastrarEntrada);

            this.Cep = cadastrarEntrada.Cep;
            this.Endereco = cadastrarEntrada.Endereco;
            this.Municipio = cadastrarEntrada.Municipio;
            this.Uf = cadastrarEntrada.Uf;
        }

        public override string ToString()
        {
            return this.Usuario?.Nome?.ToUpper();
        }
    }
}
