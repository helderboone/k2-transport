using K2.Dominio.Comandos.Entrada;

namespace K2.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um motorista
    /// </summary>
    public class Motorista
    {
        /// <summary>
        /// ID do motorista
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Número da CNH do motorista
        /// </summary>
        public string Cnh { get; private set; }

        /// <summary>
        /// Usuário associado ao cliente
        /// </summary>
        public Usuario Usuario { get; private set; }

        public string Nome { get { return Usuario?.Nome; } }

        public string Email => this.Usuario?.Email;

        public string Cpf => this.Usuario?.Cpf;

        public string Rg => this.Usuario?.Rg;

        public string Celular => this.Usuario?.Celular;

        private Motorista()
        {

        }

        public Motorista(CadastrarMotoristaEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Usuario = new Usuario(entrada);

            this.Cnh = entrada.Cnh;
        }

        public void Alterar(AlterarMotoristaEntrada entrada)
        {
            if (entrada.Invalido || entrada.Id != this.Id)
                return;

            this.Cnh = entrada.Cnh;

            this.Usuario.Alterar(entrada);
        }

        public override string ToString()
        {
            return this.Usuario?.Nome?.ToUpper();
        }
    }
}
