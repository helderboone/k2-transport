using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de um dependente de uma reserva
    /// </summary>
    public class ReservaDependenteSaida : Saida<ReservaDependenteRegistro>
    {
        public ReservaDependenteSaida(bool sucesso, IEnumerable<string> mensagens, ReservaDependenteRegistro retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public static ReservaDependenteSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ReservaDependenteSaida>(json)
                : null;
        }
    }
    public class ReservaDependenteRegistro
    {
        public int IdReserva { get; set; }

        public string Nome { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }
    }
}
