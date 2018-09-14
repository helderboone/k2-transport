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

        public string Nome { get { return Usuario?.Nome; } }

        public string Email => this.Usuario?.Email;

        public string Cpf => this.Usuario?.Cpf;

        public string Rg => this.Usuario?.Rg;

        public string Celular => this.Usuario?.Celular;

        private Cliente()
        {

        }

        public Cliente(CadastrarClienteEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Usuario = new Usuario(entrada);

            this.Cep       = entrada.Cep;
            this.Endereco  = entrada.Endereco;
            this.Municipio = entrada.Municipio;
            this.Uf        = entrada.Uf;
        }

        public void Alterar(AlterarClienteEntrada entrada)
        {
            if (entrada.Invalido || entrada.Id != this.Id)
                return;

            this.Cep       = entrada.Cep;
            this.Endereco  = entrada.Endereco;
            this.Municipio = entrada.Municipio;
            this.Uf        = entrada.Uf;

            this.Usuario.Alterar(entrada);
        }

        public override string ToString()
        {
            return this.Usuario?.Nome?.ToUpper();
        }
    }
}
