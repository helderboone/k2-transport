using K2.Dominio.Entidades;

namespace K2.Dominio.Comandos.Saida
{
    /// <summary>
    /// Classe que representa um carro
    /// </summary>
    public class CarroSaida
    {
        /// <summary>
        /// ID do carro
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// ID do proprietário
        /// </summary>
        public int IdProprietario { get; }

        /// <summary>
        /// Descrição do carro
        /// </summary>
        public string Descricao { get; }

        /// <summary>
        /// Nome do fabricante do carro
        /// </summary>
        public string NomeFabricante { get; }

        /// <summary>
        /// Ano/modelo de fabricação do carro
        /// </summary>
        public string AnoModelo { get; }

        /// <summary>
        /// Quantidade de lugares disponíveis para passageiros
        /// </summary>
        public int QuantidadeLugares { get; }

        /// <summary>
        /// Número da placa do carro
        /// </summary>
        public string Placa { get; }

        /// <summary>
        /// Número RENAVAM do carro
        /// </summary>
        public string Renavam { get; }

        /// <summary>
        /// Descrição das características do carro
        /// </summary>
        public string[] Caracteristicas { get; }

        /// <summary>
        /// Proprietário do carro
        /// </summary>
        public object Proprietario { get; }

        public CarroSaida(Carro carro)
        {
            this.Id                = carro.Id;
            this.IdProprietario    = carro.IdProprietario;
            this.Descricao         = carro.Descricao;
            this.NomeFabricante    = carro.NomeFabricante;
            this.AnoModelo         = carro.AnoModelo;
            this.QuantidadeLugares = carro.QuantidadeLugares;
            this.Placa             = carro.Placa;
            this.Renavam           = carro.Renavam;
            this.Caracteristicas   = carro.Caracteristicas?.Split(";".ToCharArray());

            this.Proprietario = new
            {
                carro.Proprietario.Nome,
                carro.Proprietario.Cpf,
                carro.Proprietario.Rg,
                carro.Proprietario.Celular
            };
        }
    }
}
