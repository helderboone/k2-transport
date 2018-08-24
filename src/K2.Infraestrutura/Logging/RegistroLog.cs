using System;

namespace K2.Infraestrutura.Logging
{
    public class RegistroLog
    {
        public int Id { get; set; }

        public string Tipo { get; set; }

        public string Mensagem { get; set; }

        public string Stacktrace { get; set; }

        public DateTime DataCriacao { get; set; }
    }
}
