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
    public class ClienteServico : Notificavel, IClienteServico
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IUow _uow;

        public ClienteServico(IClienteRepositorio clienteRepositorio, IUsuarioRepositorio usuarioRepositorio, IUow uow)
        {
            _clienteRepositorio = clienteRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _uow                = uow;
        }

        public async Task<ISaida> ObterClientePorId(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, UsuarioResource.Id_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var cliente = await _clienteRepositorio.ObterPorId(id);

            // Verifica se o cliente existe
            this.NotificarSeNulo(cliente, ClienteResource.Id_Cliente_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { ClienteResource.Cliente_Encontrado_Com_Sucesso }, new ClienteSaida(cliente));
        }

        public async Task<ISaida> ProcurarClientes(ProcurarClienteEntrada entrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return entrada.Invalido
                ? new Saida(false, entrada.Mensagens, null)
                : await _clienteRepositorio.Procurar(entrada);
        }

        public async Task<ISaida> CadastrarCliente(CadastrarClienteEntrada entrada)
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

            // Cadastra o cliente 
            var cliente = new Cliente(entrada);

            await _clienteRepositorio.Inserir(cliente);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ClienteResource.Cliente_Cadastrado_Com_Sucesso }, new ClienteSaida(cliente));
        }

        public async Task<ISaida> AlterarCliente(AlterarClienteEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var cliente = await _clienteRepositorio.ObterPorId(entrada.Id, true);

            // Verifica se a categoria existe
            this.NotificarSeNulo(cliente, ClienteResource.Id_Cliente_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe um usuário com o mesmo email
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorEmail(entrada.Email, cliente.IdUsuario), UsuarioResource.Usuario_Ja_Existe_Por_Email);

            // Verifica se já existe um usuário com o mesmo CPF
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorCpf(entrada.Cpf, cliente.IdUsuario), UsuarioResource.Usuario_Ja_Existe_Por_Cpf);

            // Verifica se já existe um usuário com o mesmo RG
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorRg(entrada.Rg, cliente.IdUsuario), UsuarioResource.Usuario_Ja_Existe_Por_Rg);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Altera o cliente
            cliente.Alterar(entrada);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ClienteResource.Cliente_Alterado_Com_Sucesso }, new ClienteSaida(cliente));
        }

        public async Task<ISaida> ExcluirCliente(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, ClienteResource.Id_Cliente_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var cliente = await _clienteRepositorio.ObterPorId(id);

            // Verifica se o cliente existe
            this.NotificarSeNulo(cliente, ClienteResource.Id_Cliente_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _usuarioRepositorio.Deletar(cliente.Usuario);

            _clienteRepositorio.Deletar(cliente);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ClienteResource.Cliente_Excluido_Com_Sucesso }, null);
        }
    }
}
