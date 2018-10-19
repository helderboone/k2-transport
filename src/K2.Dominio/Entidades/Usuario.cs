using K2.Dominio.Comandos.Entrada;
using NETCore.Encrypt.Extensions;
using System;

namespace K2.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um usuário
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        public string Senha { get; internal set; }

        /// <summary>
        /// CPF do usuário
        /// </summary>
        public string Cpf { get; internal set; }

        /// <summary>
        /// RG do usuário
        /// </summary>
        public string Rg { get; internal set; }

        /// <summary>
        /// Celular do usuário
        /// </summary>
        public string Celular { get; internal set; }

        /// <summary>
        /// Indica se o usuário está ativo
        /// </summary>
        public bool Ativo { get; private set; }

        /// <summary>
        /// Indica se o usuário é um administrador
        /// </summary>
        public bool Administrador { get; private set; }

        private Usuario()
        {

        }

        public Usuario (CadastrarUsuarioEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Nome          = entrada.Nome;
            this.Senha         = entrada.Senha.MD5();
            this.Email         = entrada.Email;
            this.Cpf           = entrada.Cpf;
            this.Rg            = entrada.Rg;
            this.Celular       = entrada.Celular;
            this.Ativo         = true;
            this.Administrador = entrada.Administrador;
        }

        public void Alterar(AlterarUsuarioEntrada entrada)
        {
            this.Nome          = entrada.Nome;
            this.Email         = entrada.Email;
            this.Cpf           = entrada.Cpf;
            this.Rg            = entrada.Rg;
            this.Celular       = entrada.Celular;
            this.Ativo         = entrada.Ativo;
            this.Administrador = entrada.Administrador;
        }

        public void Alterar(AlterarMeusDadosEntrada entrada)
        {
            this.Nome = entrada.Nome;
            this.Email = entrada.Email;
            this.Cpf = entrada.Cpf;
            this.Rg = entrada.Rg;
            this.Celular = entrada.Celular;
        }

        public string RefefinirSenha()
        {
            var senhaTemp = Guid.NewGuid().ToString().Substring(0, 8);

            this.Senha = senhaTemp.MD5();

            return senhaTemp;
        }

        public override string ToString()
        {
            return this.Nome.ToUpper();
        }
    }
}
