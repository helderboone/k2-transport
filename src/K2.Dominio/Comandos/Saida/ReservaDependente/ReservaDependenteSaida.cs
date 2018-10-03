using K2.Dominio.Entidades;
using System;

namespace K2.Dominio.Comandos.Saida
{
    /// <summary>
    /// Classe que representa um dependente de uma reserva
    /// </summary>
    public class ReservaDependenteSaida
    {
        /// <summary>
        /// ID da reserva
        /// </summary>
        public int IdReserva { get; }

        /// <summary>
        /// Nome do dependente
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Data de nascimento do dependente
        /// </summary>
        public DateTime DataNascimento { get; }

        /// <summary>
        /// CPF do dependente
        /// </summary>
        public string Cpf { get; }

        /// <summary>
        /// RG do dependente
        /// </summary>
        public string Rg { get;  }

        public ReservaDependenteSaida(ReservaDependente dependente)
        {
            IdReserva      = dependente.IdReserva;
            Nome           = dependente.Nome;
            DataNascimento = dependente.DataNascimento;
            Cpf            = dependente.Cpf;
            Rg             = dependente.Rg;
        }
    }
}
