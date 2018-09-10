﻿namespace K2.Api.ViewModels
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para alteração de um cliente
    /// </summary>
    public class AlterarClienteViewModel : AlterarUsuarioViewModel
    {
        /// <summary>
        /// CEP do cliente
        /// </summary>
        public string Cep { get; set; }

        /// <summary>
        /// Endereço do cliente
        /// </summary>
        public string Endereco { get; set; }

        /// <summary>
        /// Município do cliente
        /// </summary>
        public string Municipio { get; set; }

        /// <summary>
        /// UF do cliente
        /// </summary>
        public string Uf { get; set; }
    }
}
