using K2.Dominio.Entidades;
using System;

namespace K2.Dominio.Comandos.Saida
{
    /// <summary>
    /// Classe que representa um motorista
    /// </summary>
    public class MotoristaSaida
    {
        /// <summary>
        /// Id do motorista
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Id do Usuario
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Indica se o motorista está ativo
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
        /// Número da CNH do motorista
        /// </summary>
        public string Cnh { get; }

        /// <summary>
        /// Data de expedição da CNH do motorista
        /// </summary>
        public DateTime DataExpedicaoCnh { get; }

        /// <summary>
        /// Data de validade da CNH do motorista
        /// </summary>
        public DateTime DataValidadeCnh { get; }

        /// <summary>
        /// CEP do motorista
        /// </summary>
        public string Cep { get; }

        /// <summary>
        /// Descrição do endereço do motorista
        /// </summary>
        public string Endereco { get; }

        /// <summary>
        /// Nome do município do motorista
        /// </summary>
        public string Municipio { get; }

        /// <summary>
        /// Sigla da UF do motorista
        /// </summary>
        public string Uf { get; }

        public MotoristaSaida(Motorista motorista)
        {
            this.Id               = motorista.Id;
            this.IdUsuario        = motorista.IdUsuario;
            this.Ativo            = motorista.Usuario.Ativo;
            this.Nome             = motorista.Usuario.Nome;
            this.Email            = motorista.Usuario.Email;
            this.Cpf              = motorista.Usuario.Cpf;
            this.Rg               = motorista.Usuario.Rg;
            this.Celular          = motorista.Usuario.Celular;
            this.Cnh              = motorista.Cnh;
            this.DataExpedicaoCnh = motorista.DataExpedicaoCnh;
            this.DataValidadeCnh  = motorista.DataValidadeCnh;
            this.Cep              = motorista.Cep;
            this.Endereco         = motorista.Endereco;
            this.Municipio        = motorista.Municipio;
            this.Uf               = motorista.Uf;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
