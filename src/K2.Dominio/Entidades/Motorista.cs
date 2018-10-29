using K2.Dominio.Comandos.Entrada;
using System;

namespace K2.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um motorista
    /// </summary>
    public class Motorista
    {
        /// <summary>
        /// ID do motorista
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Número da CNH do motorista
        /// </summary>
        public string Cnh { get; private set; }

        /// <summary>
        /// Data de expedição da CNH do motorista
        /// </summary>
        public DateTime DataExpedicaoCnh { get; private set; }

        /// <summary>
        /// Data de validade da CNH do motorista
        /// </summary>
        public DateTime DataValidadeCnh { get; private set; }

        /// <summary>
        /// CEP do motorista
        /// </summary>
        public string Cep { get; private set; }

        /// <summary>
        /// Descrição do endereço do motorista
        /// </summary>
        public string Endereco { get; private set; }

        /// <summary>
        /// Nome do município do motorista
        /// </summary>
        public string Municipio { get; private set; }

        /// <summary>
        /// Sigla da UF do motorista
        /// </summary>
        public string Uf { get; private set; }

        /// <summary>
        /// Usuário associado ao motorista
        /// </summary>
        public Usuario Usuario { get; private set; }

        public string Nome { get { return Usuario?.Nome; } }

        public string Email => this.Usuario?.Email;

        public string Cpf => this.Usuario?.Cpf;

        public string Rg => this.Usuario?.Rg;

        public string Celular => this.Usuario?.Celular;

        private Motorista()
        {

        }

        public Motorista(CadastrarMotoristaEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Usuario = new Usuario(entrada);

            this.Cnh              = entrada.Cnh;
            this.DataExpedicaoCnh = entrada.DataExpedicaoCnh;
            this.DataValidadeCnh  = entrada.DataValidadeCnh;
            this.Cep              = entrada.Cep;
            this.Endereco         = entrada.Endereco;
            this.Municipio        = entrada.Municipio;
            this.Uf               = entrada.Uf;
        }

        public void Alterar(AlterarMotoristaEntrada entrada)
        {
            if (entrada.Invalido || entrada.Id != this.Id)
                return;

            this.Cnh              = entrada.Cnh;
            this.DataExpedicaoCnh = entrada.DataExpedicaoCnh;
            this.DataValidadeCnh  = entrada.DataValidadeCnh;
            this.Cep              = entrada.Cep;
            this.Endereco         = entrada.Endereco;
            this.Municipio        = entrada.Municipio;
            this.Uf               = entrada.Uf;

            this.Usuario.Alterar(entrada);
        }

        public void Alterar(AlterarMeusDadosEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Cnh = entrada.Cnh;
        }

        public override string ToString()
        {
            return this.Usuario?.Nome?.ToUpper();
        }
    }
}
