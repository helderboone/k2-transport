using System;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para o cadastro de um dependente de uma reserva
    /// </summary>
    public class CadastrarReservaDependenteEntrada : BaseModel
    {
        public int IdReserva { get; set; }

        public string Nome { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }
    }
}
