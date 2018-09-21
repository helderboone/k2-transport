﻿using JNogueira.Infraestrutura.NotifiqueMe;
using JNogueira.Infraestrutura.Utilzao;
using K2.Dominio.Resources;

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

        public CadastrarMotoristaEntrada(
            string nome,
            string email,
            string senha,
            string cpf,
            string rg,
            string celular,
            string cnh)
            : base(nome, email, senha, cpf, rg, celular)
        {
            Cnh = cnh?.RemoverCaracter(".", "-", "/");

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