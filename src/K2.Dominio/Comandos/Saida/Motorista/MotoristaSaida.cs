using K2.Dominio.Entidades;

namespace K2.Dominio.Comandos.Saida
{
    /// <summary>
    /// Classe que representa um motorista
    /// </summary>
    public class MotoristaSaida
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
        /// Indica se o motorista está ativo
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
        /// Número da CNH do motorista
        /// </summary>
        public string Cnh { get; }

        public MotoristaSaida(Motorista motorista)
        {
            this.Id        = motorista.Id;
            this.IdUsuario = motorista.IdUsuario;
            this.Ativo     = motorista.Usuario.Ativo;
            this.Nome      = motorista.Usuario.Nome;
            this.Email     = motorista.Usuario.Email;
            this.Cpf       = motorista.Usuario.Cpf;
            this.Rg        = motorista.Usuario.Rg;
            this.Celular   = motorista.Usuario.Celular;
            this.Cnh       = motorista.Cnh;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
