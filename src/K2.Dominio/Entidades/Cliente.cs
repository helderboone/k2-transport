using System;
using System.Collections.Generic;
using System.Text;

namespace K2.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um cliente
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// ID do cliente
        /// </summary>
        public int IdCliente { get; private set; }

        /// <summary>
        /// CEP do cliente
        /// </summary>
        public string Cep { get; private set; }

        /// <summary>
        /// Descrição do endereço do cliente
        /// </summary>
        public string Endereco { get; private set; }

        /// <summary>
        /// Nome do município do cliente
        /// </summary>
        public string NomeMunicipio{ get; private set; }

        /// <summary>
        /// Sigla da UF do cliente
        /// </summary>
        public string Uf { get; private set; }

        /// <summary>
        /// Usuário associado ao cliente
        /// </summary>
        public Usuario Usuario { get; private set; }

        private Cliente()
        {

        }
    }
}
