using K2.Dominio.Entidades;

namespace K2.Dominio.Comandos.Saida
{
    /// <summary>
    /// Classe que representa um cliente
    /// </summary>
    public class ClienteSaida
    {
        /// <summary>
        /// Id do cliente
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Id do Usuario
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Indica se o cliente está ativo
        /// </summary>
        public bool Ativo { get; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// CPF do usuário
        /// </summary>
        public string Cpf { get; }

        /// <summary>
        /// RG do usuário
        /// </summary>
        public string Rg { get; }

        /// <summary>
        /// Celular do usuário
        /// </summary>
        public string Celular { get; }

        /// <summary>
        /// CEP do cliente
        /// </summary>
        public string Cep { get; }

        /// <summary>
        /// Descrição do endereço do cliente
        /// </summary>
        public string Endereco { get; }

        /// <summary>
        /// Nome do município do cliente
        /// </summary>
        public string Municipio { get; }

        /// <summary>
        /// Sigla da UF do cliente
        /// </summary>
        public string Uf { get; }

        public ClienteSaida(Cliente cliente)
        {
            this.Id        = cliente.Id;
            this.IdUsuario = cliente.IdUsuario;
            this.Ativo     = cliente.Usuario.Ativo;
            this.Nome      = cliente.Usuario.Nome;
            this.Email     = cliente.Usuario.Email;
            this.Cpf       = cliente.Usuario.Cpf;
            this.Rg        = cliente.Usuario.Rg;
            this.Celular   = cliente.Usuario.Celular;
            this.Cep       = cliente.Cep;
            this.Endereco  = cliente.Endereco;
            this.Municipio = cliente.Municipio;
            this.Uf        = cliente.Uf;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
