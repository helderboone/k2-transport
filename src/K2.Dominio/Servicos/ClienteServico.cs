using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Interfaces.Dados;
using K2.Dominio.Interfaces.Dados.Repositorios;
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
            _uow = uow;
        }

        public async Task<ISaida> CadastrarCliente(CadastrarClienteEntrada cadastrarClienteEntrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (cadastrarClienteEntrada.Invalido)
                return new Saida(false, cadastrarClienteEntrada.Mensagens, null);

            // Verifica se já existe um usuário com o mesmo email
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorEmail(cadastrarClienteEntrada.Email), ClienteResource.Cliente_Ja_Existe_Por_Email);

            // Verifica se já existe um usuário com o mesmo CPF
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorCpf(cadastrarClienteEntrada.Cpf), ClienteResource.Cliente_Ja_Existe_Por_Cpf);

            // Verifica se já existe um usuário com o mesmo RG
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorRg(cadastrarClienteEntrada.Rg), ClienteResource.Cliente_Ja_Existe_Por_Rg);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Cadastra o cliente 
            var cliente = new Cliente(cadastrarClienteEntrada);

            await _clienteRepositorio.Inserir(cliente);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ClienteResource.Cliente_Cadastrado_Com_Sucesso }, new ClienteSaida(cliente));
        }

        public async Task<ISaida> ProcurarClientes(ProcurarClienteEntrada procurarEntrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return procurarEntrada.Invalido
                ? new Saida(false, procurarEntrada.Mensagens, null)
                : await _clienteRepositorio.Procurar(procurarEntrada);
        }
    }
}
