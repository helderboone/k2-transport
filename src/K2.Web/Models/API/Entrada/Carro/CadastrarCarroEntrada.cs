namespace K2.Web.Models
{
    /// <summary>
    /// Comando utilizado para o cadastro de um carro
    /// </summary>
    public class CadastrarCarroEntrada : BaseModel
    {
        public int IdProprietario { get; set; }

        public string Descricao { get; set; }

        public string NomeFabricante { get; set; }

        public string AnoModelo { get; set; }

        public int Capacidade { get; set; }

        public string Placa { get; set; }

        public string Renavam { get; set; }

        public string Cor { get; set; }

        public string NumeroRegistroSeturb { get; set; }

        public string[] Caracteristicas { get; set; }
    }
}
