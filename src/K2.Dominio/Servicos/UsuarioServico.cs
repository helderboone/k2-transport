using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Interfaces.Dados;
using K2.Dominio.Interfaces.Infraestrutura;
using K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios;
using K2.Dominio.Interfaces.Servicos;
using K2.Dominio.Resources;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace K2.Dominio.Servicos
{
    public class UsuarioServico : Notificavel, IUsuarioServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IEmailHelper _emailUtil;
        private readonly ILogger _logger;
        private readonly IUow _uow;

        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio, IUow uow, IEmailHelper emailUtil, ILogger<UsuarioServico> logger)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _uow = uow;
            _emailUtil = emailUtil;
            _logger = logger;
        }

        public async Task<ISaida> Autenticar(AutenticarUsuarioEntrada entrada)
        {
            // Verifica se o e-mail e a senha do usuário foi informado.
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var usuario = await _usuarioRepositorio.ObterPorEmailSenha(entrada.Email, entrada.Senha);

            // Verifica se o usuário com o e-mail e a senha (hash) foi encontrado no banco
            this.NotificarSeNulo(usuario, UsuarioResource.Usuario_Nao_Encontrado_Por_Login_Senha);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário está ativo
            this.NotificarSeFalso(usuario.Ativo, UsuarioResource.Usuario_Inativo);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var perfil = string.Empty;

            if (usuario.Administrador)
                usuario.Perfil = TipoPerfil.Administrador;

            return new Saida(true, new[] { UsuarioResource.Usuario_Autenticado_Com_Sucesso }, new UsuarioSaida(usuario));
        }

        public async Task<ISaida> AlterarSenha(AlterarSenhaUsuarioEntrada entrada)
        {
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var usuario = await _usuarioRepositorio.ObterPorEmailSenha(entrada.Email, entrada.SenhaAtual, true);

            this.NotificarSeNulo(usuario, UsuarioResource.Usuario_Nao_Encontrado_Por_Senha);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            usuario.Senha = entrada.CriptografarSenhaNova();

            await _uow.Commit();

            if (entrada.EnviarEmailSenhaNova)
            {
                try
                {
                    _emailUtil.EnviarEmail("teste-utilzao@jnogueira.net.br", new[] { usuario.Email }, "Senha de acesso alterada.", $"Sua nova senha de acesso é <b>{entrada.SenhaNova}</b>");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao enviar a senha alterada para o e-mail { usuario.Email }.");

                    return new Saida(true, new[] { UsuarioResource.Senha_Alterada_Com_Erro_Envio_Email }, null);
                }
            }

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { UsuarioResource.Senha_Alterada_Com_Sucesso }, null);
        }
    }
}
