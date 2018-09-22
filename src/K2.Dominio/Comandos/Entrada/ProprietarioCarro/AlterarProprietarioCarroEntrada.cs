namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o alterar um proprietário
    /// </summary>
    public class AlterarProprietarioCarroEntrada : AlterarUsuarioEntrada
    {
        /// <summary>
        /// ID do proprietário
        /// </summary>
        public int Id { get; }

        public AlterarProprietarioCarroEntrada(
            int id,
            string nome,
            string email,
            string cpf,
            string rg,
            string celular,
            bool ativo)
            : base(id, nome, email, cpf, rg, celular, ativo, false)
        {
            Id  = id;
        }

        public override string ToString()
        {
            return this.Nome.ToUpper();
        }
    }
}
