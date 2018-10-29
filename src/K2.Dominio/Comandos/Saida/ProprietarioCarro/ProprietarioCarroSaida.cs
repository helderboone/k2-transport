using K2.Dominio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace K2.Dominio.Comandos.Saida
{
    /// <summary>
    /// Classe que representa um proprietário
    /// </summary>
    public class ProprietarioCarroSaida
    {
        /// <summary>
        /// Id do proprietário
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Id do Usuario
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Indica se o proprietário está ativo
        /// </summary>
        public bool Ativo { get; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// CPF do usuário
        /// </summary>
        public string Cpf { get; }

        /// <summary>
        /// RG do usuário
        /// </summary>
        public string Rg { get; }

        /// <summary>
        /// Celular do usuário
        /// </summary>
        public string Celular { get; }

        /// <summary>
        /// Carros do proprietário
        /// </summary>
        public IEnumerable<object> Carros { get; }

        public ProprietarioCarroSaida(ProprietarioCarro proprietario)
        {
            this.Id        = proprietario.Id;
            this.IdUsuario = proprietario.IdUsuario;
            this.Ativo     = proprietario.Usuario.Ativo;
            this.Nome      = proprietario.Usuario.Nome;
            this.Email     = proprietario.Usuario.Email;
            this.Cpf       = proprietario.Usuario.Cpf;
            this.Rg        = proprietario.Usuario.Rg;
            this.Celular   = proprietario.Usuario.Celular;
            this.Carros = proprietario.Carros.Select(x => new
            {
                x.Id,
                x.Descricao,
                x.NomeFabricante,
                x.AnoModelo,
                x.Capacidade,
                x.Placa,
                x.Cor,
                x.Renavam,
                x.RegistroSeturb,
                Caracteristicas = !string.IsNullOrEmpty(x.Caracteristicas)
                    ? x.Caracteristicas.Split(";".ToCharArray())
                    : null
            }).ToList();
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
