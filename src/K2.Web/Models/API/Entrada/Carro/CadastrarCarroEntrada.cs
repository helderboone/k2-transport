namespace K2.Web.Models
{
    /// <summary>
    /// Comando utilizado para o cadastro de um carro
    /// </summary>
    public class CadastrarCarroEntrada : BaseModel
    {
        /// <summary>
        /// ID do proprietário
        /// </summary>
        public int IdProprietario { get; set; }

        /// <summary>
        /// Descrição do carro
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Nome do fabricante do carro
        /// </summary>
        public string NomeFabricante { get; set; }

        /// <summary>
        /// Ano/modelo de fabricação do carro
        /// </summary>
        public string AnoModelo { get; set; }

        /// <summary>
        /// Quantidade de lugares disponíveis para passageiros
        /// </summary>
        public int QuantidadeLugares { get; set; }

        /// <summary>
        /// Número da placa do carro
        /// </summary>
        public string Placa { get; set; }

        /// <summary>
        /// Número RENAVAM do carro
        /// </summary>
        public string Renavam { get; set; }

        /// <summary>
        /// Descrição das características do carro
        /// </summary>
        public string[] Caracteristicas { get; set; }
    }
}
