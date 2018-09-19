using JNogueira.Infraestrutura.NotifiqueMe;
using JNogueira.Infraestrutura.Utilzao;
using K2.Dominio.Resources;

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
        /// Número da CNH do motorista
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
            : base(id, nome, email, cpf, rg, celular, ativo)
        {
            Id  = id;
            Cnh = cnh?.RemoverCaracter(".", "-", "/");

            this.Validar();
        }

        public override string ToString()
        {
            return this.Nome.ToUpper();
        }

        private void Validar()
        {
            this.NotificarSeNuloOuVazio(this.Cnh, MotoristaResource.Cnh_Obrigatorio_Nao_Informado);
        }
    }
}
