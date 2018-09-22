namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de um proprietário
    /// </summary>
    public class CadastrarProprietarioCarroEntrada : CadastrarUsuarioEntrada
    {
        public CadastrarProprietarioCarroEntrada(
            string nome,
            string email,
            string senha,
            string cpf,
            string rg,
            string celular)
            : base(nome, email, senha, cpf, rg, celular)
        {

        }

        public override string ToString()
        {
            return this.Nome.ToUpper();
        }
    }
}
