namespace K2.Web.Models
{
    /// <summary>
    /// Comando utilizado para o alterar um carro
    /// </summary>
    public class AlterarCarroEntrada : BaseModel
    {
        public int Id { get; set; }

        public int IdProprietario { get; set; }

        public string Descricao { get; set; }

        public string NomeFabricante { get; set; }

        public string AnoModelo { get; set; }

        public int QuantidadeLugares { get; set; }

        public string Placa { get; set; }

        public string Renavam { get; set; }

        public string[] Caracteristicas { get; set; }
    }
}
