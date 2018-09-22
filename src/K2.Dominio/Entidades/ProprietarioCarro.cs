using K2.Dominio.Comandos.Entrada;

namespace K2.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um proprietário de carro
    /// </summary>
    public class ProprietarioCarro
    {
        /// <summary>
        /// ID do proprietário
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Usuário associado ao proprietário
        /// </summary>
        public Usuario Usuario { get; private set; }

        public string Nome { get { return Usuario?.Nome; } }

        public string Email => this.Usuario?.Email;

        public string Cpf => this.Usuario?.Cpf;

        public string Rg => this.Usuario?.Rg;

        public string Celular => this.Usuario?.Celular;

        private ProprietarioCarro()
        {

        }

        public ProprietarioCarro(CadastrarProprietarioCarroEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Usuario = new Usuario(entrada);
        }

        public void Alterar(AlterarProprietarioCarroEntrada entrada)
        {
            if (entrada.Invalido || entrada.Id != this.Id)
                return;

            this.Usuario.Alterar(entrada);
        }

        public override string ToString()
        {
            return this.Usuario?.Nome?.ToUpper();
        }
    }
}
