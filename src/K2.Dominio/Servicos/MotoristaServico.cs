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
    public class MotoristaServico : Notificavel, IMotoristaServico
    {
        private readonly IMotoristaRepositorio _motoristaRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IUow _uow;

        public MotoristaServico(IMotoristaRepositorio motoristaRepositorio, IUsuarioRepositorio usuarioRepositorio, IUow uow)
        {
            _motoristaRepositorio = motoristaRepositorio;
            _usuarioRepositorio   = usuarioRepositorio;
            _uow                  = uow;
        }

        public async Task<ISaida> ObterMotoristaPorId(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, UsuarioResource.Id_Invalido);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var motorista = await _motoristaRepositorio.ObterPorId(id);

            // Verifica se o motorista existe
            this.NotificarSeNulo(motorista, MotoristaResource.Id_Motorista_Nao_Existe);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { MotoristaResource.Motorista_Encontrado_Com_Sucesso }, new MotoristaSaida(motorista));
        }

        public async Task<ISaida> ProcurarMotoristas(ProcurarMotoristaEntrada entrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return entrada.Invalido
                ? new Saida(false, entrada.Mensagens, null)
                : await _motoristaRepositorio.Procurar(entrada);
        }

        public async Task<ISaida> CadastrarMotorista(CadastrarMotoristaEntrada entrada)
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

            // Cadastra o motorista 
            var motorista = new Motorista(entrada);

            await _motoristaRepositorio.Inserir(motorista);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { MotoristaResource.Motorista_Cadastrado_Com_Sucesso }, new MotoristaSaida(motorista));
        }

        public async Task<ISaida> AlterarMotorista(AlterarMotoristaEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var motorista = await _motoristaRepositorio.ObterPorId(entrada.Id, true);

            // Verifica se o motorista existe
            this.NotificarSeNulo(motorista, MotoristaResource.Id_Motorista_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe um usuário com o mesmo email
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorEmail(entrada.Email, motorista.IdUsuario), UsuarioResource.Usuario_Ja_Existe_Por_Email);

            // Verifica se já existe um usuário com o mesmo CPF
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorCpf(entrada.Cpf, motorista.IdUsuario), UsuarioResource.Usuario_Ja_Existe_Por_Cpf);

            // Verifica se já existe um usuário com o mesmo RG
            this.NotificarSeVerdadeiro(await _usuarioRepositorio.VerificarExistenciaPorRg(entrada.Rg, motorista.IdUsuario), UsuarioResource.Usuario_Ja_Existe_Por_Rg);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Altera o motorista
            motorista.Alterar(entrada);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { MotoristaResource.Motorista_Alterado_Com_Sucesso }, new MotoristaSaida(motorista));
        }

        public async Task<ISaida> ExcluirMotorista(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, MotoristaResource.Id_Motorista_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var motorista = await _motoristaRepositorio.ObterPorId(id);

            // Verifica se o motorista existe
            this.NotificarSeNulo(motorista, MotoristaResource.Id_Motorista_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _usuarioRepositorio.Deletar(motorista.Usuario);

            _motoristaRepositorio.Deletar(motorista);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { MotoristaResource.Motorista_Excluido_Com_Sucesso }, null);
        }
    }
}
