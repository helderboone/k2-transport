using K2.Dominio.Entidades;

namespace K2.Dominio.Comandos.Saida
{
    /// <summary>
    /// Classe que representa uma localidade
    /// </summary>
    public class LocalidadeSaida
    {
        /// <summary>
        /// Id da localidade
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Nome da localidade
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Sigla da localidade
        /// </summary>
        public string Sigla { get; }

        /// <summary>
        /// UF da localidade
        /// </summary>
        public string Uf { get; }

        public LocalidadeSaida(Localidade localidade)
        {
            this.Id    = localidade.Id;
            this.Nome  = localidade.Nome;
            this.Sigla = localidade.Sigla;
            this.Uf    = localidade.Uf;
        }

        public override string ToString()
        {
            return $"{this.Sigla} - {this.Nome}/{this.Uf}";
        }
    }
}
