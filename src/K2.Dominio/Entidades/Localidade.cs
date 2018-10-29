using K2.Dominio.Comandos.Entrada;

namespace K2.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma localidade
    /// </summary>
    public class Localidade
    {
        /// <summary>
        /// ID da localidade
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Nome da localidade
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Sigla da localidade
        /// </summary>
        public string Sigla { get; private set; }

        /// <summary>
        /// Sigla da UF da localidade
        /// </summary>
        public string Uf { get; private set; }

        private Localidade()
        {

        }

        public Localidade(CadastrarLocalidadeEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Nome  = entrada.Nome;
            this.Uf    = entrada.Uf;
            this.Sigla = entrada.Sigla;
        }

        public void Alterar(AlterarLocalidadeEntrada entrada)
        {
            if (entrada.Invalido || entrada.Id != this.Id)
                return;

            this.Nome  = entrada.Nome;
            this.Uf    = entrada.Uf;
            this.Sigla = entrada.Sigla;
        }

        public override string ToString()
        {
            return $"{this.Sigla} - {this.Nome}/{this.Uf}";
        }
    }
}
