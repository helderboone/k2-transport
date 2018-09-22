using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
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
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IMotoristaRepositorio _motoristaRepositorio;
        private readonly IProprietarioCarroRepositorio _proprietarioCarroRepositorio;
        private readonly IEmailHelper _emailUtil;
        private readonly ILogger _logger;
        private readonly IUow _uow;

        public UsuarioServico(
            IUsuarioRepositorio usuarioRepositorio,
            IClienteRepositorio clienteRepositorio,
            IMotoristaRepositorio motoristaRepositorio,
            IProprietarioCarroRepositorio proprietarioCarroRepositorio,
            IUow uow,
            IEmailHelper emailUtil,
            ILogger<UsuarioServico> logger)
        {
            _usuarioRepositorio           = usuarioRepositorio;
            _clienteRepositorio           = clienteRepositorio;
            _motoristaRepositorio         = motoristaRepositorio;
            _proprietarioCarroRepositorio = proprietarioCarroRepositorio;
            _uow                          = uow;
            _emailUtil                    = emailUtil;
            _logger                       = logger;
        }

        public async Task<ISaida> ObterUsuarioPorId(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, UsuarioResource.Id_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var usuario = await _usuarioRepositorio.ObterPorId(id);

            // Verifica se o usuário existe
            this.NotificarSeNulo(usuario, UsuarioResource.Id_Usuario_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            UsuarioSaida usuarioSaida = null;

            if (usuario.Administrador)
                usuarioSaida = new UsuarioSaida(usuario, TipoPerfil.Administrador);
            else if (await _clienteRepositorio.VerificarExistenciaPorIdUsuario(usuario.Id))
                usuarioSaida = new UsuarioSaida(usuario, TipoPerfil.Cliente);
            else if (await _motoristaRepositorio.VerificarExistenciaPorIdUsuario(usuario.Id))
                usuarioSaida = new UsuarioSaida(usuario, TipoPerfil.Motorista);
            else if (await _proprietarioCarroRepositorio.VerificarExistenciaPorIdUsuario(usuario.Id))
                usuarioSaida = new UsuarioSaida(usuario, TipoPerfil.ProprietarioCarro);

            return new Saida(true, new[] { UsuarioResource.Usuario_Encontrado_Com_Sucesso }, usuarioSaida);
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

            UsuarioSaida usuarioSaida = null;

            if (usuario.Administrador)
                usuarioSaida = new UsuarioSaida(usuario, TipoPerfil.Administrador);
            else if (await _clienteRepositorio.VerificarExistenciaPorIdUsuario(usuario.Id))
                usuarioSaida = new UsuarioSaida(usuario, TipoPerfil.Cliente);
            else if (await _motoristaRepositorio.VerificarExistenciaPorIdUsuario(usuario.Id))
                usuarioSaida = new UsuarioSaida(usuario, TipoPerfil.Motorista);
            else if (await _proprietarioCarroRepositorio.VerificarExistenciaPorIdUsuario(usuario.Id))
                usuarioSaida = new UsuarioSaida(usuario, TipoPerfil.ProprietarioCarro);

            return new Saida(true, new[] { UsuarioResource.Usuario_Autenticado_Com_Sucesso }, usuarioSaida);
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

            if (!_uow.Invalido && entrada.EnviarEmailSenhaNova)
            {
                try
                {
                    _emailUtil.EnviarEmail(new[] { usuario.Email }, "Senha de acesso alterada.", $"Sua nova senha de acesso é <b>{entrada.SenhaNova}</b>");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao enviar a senha alterada para o e-mail { usuario.Email }.");

                    return new Saida(true, new[] { UsuarioResource.Senha_Alterada_Com_Erro_Envio_Email }, null);
                }

                new Saida(true, new[] { UsuarioResource.Senha_Alterada_Com_Sucesso }, null);
            }

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { UsuarioResource.Senha_Alterada_Com_Sucesso }, null);
        }

        public async Task<ISaida> RedefinirSenha(int id)
        {
            var usuario = await _usuarioRepositorio.ObterPorId(id, true);

            this.NotificarSeNulo(usuario, UsuarioResource.Id_Usuario_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var senhaTemporaria = usuario.RefefinirSenha();

            await _uow.Commit();

            if (!_uow.Invalido)
            {
                try
                {
                    _emailUtil.EnviarEmail(new[] { usuario.Email }, "Senha de acesso temporária.", $"Sua senha de acesso foi alterada para <b>{senhaTemporaria}</b>. Acesse o sistema e altera sua senha de acordo com a suas preferências.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Erro ao enviar a senha redefinida para o e-mail { usuario.Email }.");

                    return new Saida(true, new[] { UsuarioResource.Senha_Redefinida_Com_Erro_Envio_Email }, new { SenhaTemporaria = senhaTemporaria });
                }

                return new Saida(true, new[] { UsuarioResource.Senha_Redefinida_Com_Sucesso }, new { SenhaTemporaria = senhaTemporaria });
            }

            return new Saida(false, _uow.Mensagens, null);
        }

        public async Task<ISaida> ProcurarUsuarios(ProcurarUsuarioEntrada entrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return entrada.Invalido
                ? new Saida(false, entrada.Mensagens, null)
                : await _usuarioRepositorio.Procurar(entrada);
        }

        public async Task<ISaida> CadastrarUsuario(CadastrarUsuarioEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se já existe um usuário com o mesmo email
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorEmail(entrada.Email), UsuarioResource.Usuario_Ja_Existe_Por_Email);

            // Verifica se já existe um usuário com o mesmo CPF
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorCpf(entrada.Cpf), UsuarioResource.Usuario_Ja_Existe_Por_Cpf);

            // Verifica se já existe um usuário com o mesmo RG
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorRg(entrada.Rg), UsuarioResource.Usuario_Ja_Existe_Por_Rg);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Cadastra o usuário 
            var usuario = new Usuario(entrada);

            await _usuarioRepositorio.Inserir(usuario);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { UsuarioResource.Usuario_Cadastrado_Com_Sucesso }, new UsuarioSaida(usuario));
        }

        public async Task<ISaida> AlterarUsuario(AlterarUsuarioEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var usuario = await _usuarioRepositorio.ObterPorId(entrada.IdUsuario, true);

            // Verifica se o usuário existe
            this.NotificarSeNulo(usuario, UsuarioResource.Id_Usuario_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe um usuário com o mesmo email
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorEmail(entrada.Email, usuario.Id), UsuarioResource.Usuario_Ja_Existe_Por_Email);

            // Verifica se já existe um usuário com o mesmo CPF
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorCpf(entrada.Cpf, usuario.Id), UsuarioResource.Usuario_Ja_Existe_Por_Cpf);

            // Verifica se já existe um usuário com o mesmo RG
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorRg(entrada.Rg, usuario.Id), UsuarioResource.Usuario_Ja_Existe_Por_Rg);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Altera o cliente
            usuario.Alterar(entrada);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { UsuarioResource.Usuario_Alterado_Com_Sucesso }, new UsuarioSaida(usuario));
        }

        public async Task<ISaida> ExcluirUsuario(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, UsuarioResource.Id_Usuario_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var usuario = await _usuarioRepositorio.ObterPorId(id);

            // Verifica se o usuario existe
            this.NotificarSeNulo(usuario, UsuarioResource.Id_Usuario_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _usuarioRepositorio.Deletar(usuario);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { UsuarioResource.Usuario_Excluido_Com_Sucesso }, null);
        }
    }
}
