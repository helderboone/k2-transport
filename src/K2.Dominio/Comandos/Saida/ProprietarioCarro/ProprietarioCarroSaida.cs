using K2.Dominio.Entidades;

namespace K2.Dominio.Comandos.Saida
{
    /// <summary>
    /// Classe que representa um proprietário
    /// </summary>
    public class ProprietarioCarroSaida
    {
        /// <summary>
        /// Id do proprietário
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Id do Usuario
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Indica se o proprietário está ativo
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

        public ProprietarioCarroSaida(ProprietarioCarro proprietario)
        {
            this.Id        = proprietario.Id;
            this.IdUsuario = proprietario.IdUsuario;
            this.Ativo     = proprietario.Usuario.Ativo;
            this.Nome      = proprietario.Usuario.Nome;
            this.Email     = proprietario.Usuario.Email;
            this.Cpf       = proprietario.Usuario.Cpf;
            this.Rg        = proprietario.Usuario.Rg;
            this.Celular   = proprietario.Usuario.Celular;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
