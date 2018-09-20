﻿namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para realizar a procura por clientes
    /// </summary>
    public class ProcurarClienteEntrada : ProcurarEntrada
    {
        /// <summary>
        /// Nome do cliente
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// E-mail do cliente
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// CPF do cliente
        /// </summary>
        public string Cpf { get; set; }

        /// <summary>
        /// Número do RG do cliente
        /// </summary>
        public string Rg { get; set; }
    }
}
