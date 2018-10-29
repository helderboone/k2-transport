using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Resources;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de uma localidade
    /// </summary>
    public class CadastrarLocalidadeEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Nome da localidade
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Sigla da localidade
        /// </summary>
        public string Sigla { get; }

        /// <summary>
        /// Sigla da UF da localidade
        /// </summary>
        public string Uf { get; }

        public CadastrarLocalidadeEntrada(
            string nome,
            string sigla,
            string uf)
        {
            Nome  = nome;
            Sigla = sigla?.ToUpper();
            Uf    = uf?.ToUpper();

            this.Validar();
        }

        public override string ToString()
        {
            return $"{this.Sigla} - {this.Nome}/{this.Uf}";
        }

        private void Validar()
        {
            this
                .NotificarSeNuloOuVazio(this.Nome, LocalidadeResource.Nome_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.Sigla, LocalidadeResource.Sigla_Obrigatoria_Nao_Informada)
                .NotificarSeNuloOuVazio(this.Uf, LocalidadeResource.Uf_Obrigatoria_Nao_Informado);
        }
    }
}
