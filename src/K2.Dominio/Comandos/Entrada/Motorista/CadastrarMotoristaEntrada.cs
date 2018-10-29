using JNogueira.Infraestrutura.NotifiqueMe;
using JNogueira.Infraestrutura.Utilzao;
using K2.Dominio.Resources;
using System;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de um motorista
    /// </summary>
    public class CadastrarMotoristaEntrada : CadastrarUsuarioEntrada
    {
        /// <summary>
        /// CNH do cliente
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

        public CadastrarMotoristaEntrada(
            string nome,
            string email,
            string senha,
            string cpf,
            string rg,
            string celular,
            string cnh,
            DateTime dataExpedicaoCnh,
            DateTime dataValidadeCnh,
            string cep = null,
            string endereco = null,
            string municipio = null,
            string uf = null
            )
            : base(nome, email, senha, cpf, rg, celular)
        {
            Cnh              = cnh?.RemoverCaracter(".", "-", "/");
            DataExpedicaoCnh = dataExpedicaoCnh;
            DataValidadeCnh  = dataValidadeCnh;
            Cep              = cep?.RemoverCaracter(".", "-", "/");
            Endereco         = endereco?.ToUpper();
            Municipio        = municipio?.ToUpper();
            Uf               = uf?.ToUpper();

            this.Validar();
        }

        public override string ToString()
        {
            return this.Nome.ToUpper();
        }

        private void Validar()
        {
            this.NotificarSeNuloOuVazio(this.Cnh, MotoristaResource.Cnh_Obrigatorio_Nao_Informado);
        }
    }
}
