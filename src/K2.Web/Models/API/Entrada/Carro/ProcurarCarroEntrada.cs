namespace K2.Web.Models
{
    public class ProcurarCarroEntrada : ProcurarEntrada
    {
        public int? IdProprietario { get; set; }

        public string Descricao { get; set; }

        public string NomeFabricante { get; set; }

        public string AnoModelo { get; set; }

        public string Placa { get; set; }

        public string Renavam { get; set; }
    }
}
