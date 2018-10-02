using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Interfaces.Dados;
using K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios;
using K2.Dominio.Interfaces.Servicos;
using K2.Dominio.Resources;
using System.Threading.Tasks;

namespace K2.Dominio.Servicos
{
    public class ProprietarioCarroServico : Notificavel, IProprietarioCarroServico
    {
        private readonly IProprietarioCarroRepositorio _proprietarioCarroRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IUow _uow;

        public ProprietarioCarroServico(IProprietarioCarroRepositorio proprietarioCarroRepositorio, IUsuarioRepositorio usuarioRepositorio, IUow uow)
        {
            _proprietarioCarroRepositorio = proprietarioCarroRepositorio;
            _usuarioRepositorio           = usuarioRepositorio;
            _uow                          = uow;
        }

        public async Task<ISaida> ObterProprietarioCarroPorId(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, UsuarioResource.Id_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var proprietario = await _proprietarioCarroRepositorio.ObterPorId(id);

            // Verifica se o proprietário existe
            this.NotificarSeNulo(proprietario, ProprietarioCarroResource.Id_Proprietario_Nao_Existe);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { ProprietarioCarroResource.Proprietario_Encontrado_Com_Sucesso }, new ProprietarioCarroSaida(proprietario));
        }

        public async Task<ISaida> ProcurarProprietarioCarros(ProcurarProprietarioCarroEntrada entrada)
        {
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se os parâmetros para a procura foram informadas corretamente
            return entrada.Invalido
                ? new Saida(false, entrada.Mensagens, null)
                : await _proprietarioCarroRepositorio.Procurar(entrada);
        }

        public async Task<ISaida> CadastrarProprietarioCarro(CadastrarProprietarioCarroEntrada entrada)
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

            // Cadastra o proprietário 
            var proprietario = new ProprietarioCarro(entrada);

            await _proprietarioCarroRepositorio.Inserir(proprietario);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ProprietarioCarroResource.Proprietario_Cadastrado_Com_Sucesso }, new ProprietarioCarroSaida(proprietario));
        }

        public async Task<ISaida> AlterarProprietarioCarro(AlterarProprietarioCarroEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var proprietario = await _proprietarioCarroRepositorio.ObterPorId(entrada.Id, true);

            // Verifica se o proprietário existe
            this.NotificarSeNulo(proprietario, ProprietarioCarroResource.Id_Proprietario_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe um usuário com o mesmo email
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorEmail(entrada.Email, proprietario.IdUsuario), UsuarioResource.Usuario_Ja_Existe_Por_Email);

            // Verifica se já existe um usuário com o mesmo CPF
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorCpf(entrada.Cpf, proprietario.IdUsuario), UsuarioResource.Usuario_Ja_Existe_Por_Cpf);

            // Verifica se já existe um usuário com o mesmo RG
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorRg(entrada.Rg, proprietario.IdUsuario), UsuarioResource.Usuario_Ja_Existe_Por_Rg);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Altera o proprietário
            proprietario.Alterar(entrada);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ProprietarioCarroResource.Proprietario_Alterado_Com_Sucesso }, new ProprietarioCarroSaida(proprietario));
        }

        public async Task<ISaida> ExcluirProprietarioCarro(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, ProprietarioCarroResource.Id_Proprietario_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var proprietario = await _proprietarioCarroRepositorio.ObterPorId(id);

            // Verifica se o proprietário existe
            this.NotificarSeNulo(proprietario, ProprietarioCarroResource.Id_Proprietario_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _usuarioRepositorio.Deletar(proprietario.Usuario);

            _proprietarioCarroRepositorio.Deletar(proprietario);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ProprietarioCarroResource.Proprietario_Excluido_Com_Sucesso }, null);
        }
    }
}
