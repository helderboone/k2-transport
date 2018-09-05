﻿namespace K2.Api.ViewModels.ViewModels
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para o cadastro de um usuário
    /// </summary>
    public class CadastrarUsuarioViewModel
    {
        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Senha de acesso do usuário
        /// </summary>
        public string Senha { get; set; }

        /// <summary>
        /// CPF do usuário
        /// </summary>
        public string Cpf { get; set; }

        /// <summary>
        /// Número do RG do usuário
        /// </summary>
        public string Rg { get; set; }

        /// <summary>
        /// Número do celular do usuário
        /// </summary>
        public string Celular { get; set; }
    }
}
