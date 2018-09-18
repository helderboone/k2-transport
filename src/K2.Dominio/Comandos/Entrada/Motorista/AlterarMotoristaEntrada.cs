using JNogueira.Infraestrutura.Utilzao;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o alterar um motorista
    /// </summary>
    public class AlterarMotoristaEntrada : AlterarUsuarioEntrada
    {
        /// <summary>
        /// ID do motorista
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Número da CNH do cliente
        /// </summary>
        public string Cnh { get; }

        public AlterarMotoristaEntrada(
            int id,
            string nome,
            string email,
            string cpf,
            string rg,
            string celular,
            bool ativo,
            string cnh)
            : base(nome, email, cpf, rg, celular, ativo)
        {
            Id  = id;
            Cnh = cnh?.RemoverCaracter(".", "-", "/");
        }

        public override string ToString()
        {
            return this.Nome.ToUpper();
        }
    }
}
