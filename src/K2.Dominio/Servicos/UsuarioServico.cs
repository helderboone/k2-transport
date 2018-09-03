using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Comandos.Entrada.Usuario;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Interfaces.Dados;
using K2.Dominio.Interfaces.Dados.Repositorios;
using K2.Dominio.Interfaces.Servicos;
using K2.Dominio.Resources;
using System.Threading.Tasks;

namespace K2.Dominio.Servicos
{
    public class UsuarioServico : Notificavel, IUsuarioServico
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IUow _uow;

        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio, IUow uow)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _uow = uow;
        }

        public async Task<ISaida> Autenticar(AutenticarUsuarioEntrada autenticacaoEntrada)
        {
            // Verifica se o e-mail e a senha do usuário foi informado.
            if (autenticacaoEntrada.Invalido)
                return new Saida(false, autenticacaoEntrada.Mensagens, null);

            var usuario = await _usuarioRepositorio.ObterPorEmailSenha(autenticacaoEntrada.Email, autenticacaoEntrada.Senha);

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
                perfil = Perfil.Administrador;

            usuario.Perfis = new[] { perfil };

            return new Saida(true, new[] { UsuarioResource.Usuario_Autenticado_Com_Sucesso }, new UsuarioSaida(usuario));
        }

        public async Task<ISaida> AlterarSenha(AlterarSenhaUsuarioEntrada alterarSenhaEntrada)
        {
            if (alterarSenhaEntrada.Invalido)
                return new Saida(false, alterarSenhaEntrada.Mensagens, null);

            var usuario = await _usuarioRepositorio.ObterPorEmailSenha(alterarSenhaEntrada.Email, alterarSenhaEntrada.SenhaAtual, true);

            this.NotificarSeNulo(usuario, UsuarioResource.Usuario_Nao_Encontrado_Por_Senha);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            usuario.Senha = alterarSenhaEntrada.SenhaNova;

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { UsuarioResource.Senha_Alterada_Com_Sucesso }, null);
        }
    }
}
