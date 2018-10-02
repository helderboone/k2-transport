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
    public class CarroServico : Notificavel, ICarroServico
    {
        private readonly ICarroRepositorio _carroRepositorio;
        private readonly IProprietarioCarroRepositorio _proprietarioCarroRepositorio;
        private readonly IUow _uow;

        public CarroServico(ICarroRepositorio carroRepositorio, IProprietarioCarroRepositorio proprietarioCarroRepositorio, IUow uow)
        {
            _carroRepositorio             = carroRepositorio;
            _proprietarioCarroRepositorio = proprietarioCarroRepositorio;
            _uow                          = uow;
        }

        public async Task<ISaida> ObterCarroPorId(int id, CredencialUsuarioEntrada credencial)
        {
            this.NotificarSeMenorOuIgualA(id, 0, CarroResource.Id_Carro_Nao_Existe);
            this.NotificarSeNulo(credencial, SharedResource.Credenciais_Usuario_Obrigatorias_Nao_Informadas);

            this.AdicionarNotificacoes(credencial?.Notificacoes);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var carro = await _carroRepositorio.ObterPorId(id);

            // Verifica se o carro existe
            this.NotificarSeNulo(carro, CarroResource.Id_Carro_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário possui as permissões para obter a informação.
            switch (credencial.PerfilUsuario)
            {
                case TipoPerfil.ProprietarioCarro:
                    this.NotificarSeVerdadeiro(carro.Proprietario.IdUsuario != credencial.IdUsuario, CarroResource.Proprietario_Sem_Permissao_Obter);
                    break;
            }

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { CarroResource.Carro_Encontrado_Com_Sucesso }, new CarroSaida(carro));
        }

        public async Task<ISaida> ProcurarCarros(ProcurarCarroEntrada entrada, CredencialUsuarioEntrada credencial)
        {
            this.NotificarSeNulo(credencial, SharedResource.Credenciais_Usuario_Obrigatorias_Nao_Informadas);

            this.AdicionarNotificacoes(entrada?.Notificacoes);
            this.AdicionarNotificacoes(credencial?.Notificacoes);

            // Verifica se os parâmetros para a procura foram informadas corretamente
            return this.Invalido 
                ? new Saida(false, this.Mensagens, null)
                : await _carroRepositorio.Procurar(entrada, credencial);
        }

        public async Task<ISaida> CadastrarCarro(CadastrarCarroEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se o proprietário com o ID informado existe
            this.NotificarSeFalso(await _proprietarioCarroRepositorio.VerificarExistenciaPorId(entrada.IdProprietario), ProprietarioCarroResource.Id_Proprietario_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Cadastra o carro 
            var carro = new Carro(entrada);

            await _carroRepositorio.Inserir(carro);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { CarroResource.Carro_Cadastrado_Com_Sucesso }, new CarroSaida(await _carroRepositorio.ObterPorId(carro.Id)));
        }

        public async Task<ISaida> AlterarCarro(AlterarCarroEntrada entrada, CredencialUsuarioEntrada credencial)
        {
            this.NotificarSeNulo(credencial, SharedResource.Credenciais_Usuario_Obrigatorias_Nao_Informadas);

            this.AdicionarNotificacoes(entrada?.Notificacoes);
            this.AdicionarNotificacoes(credencial?.Notificacoes);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var carro = await _carroRepositorio.ObterPorId(entrada.Id, true);

            // Verifica se o carro existe
            this.NotificarSeNulo(carro, CarroResource.Id_Carro_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o proprietário com o ID informado existe
            this.NotificarSeFalso(await _proprietarioCarroRepositorio.VerificarExistenciaPorId(entrada.IdProprietario), ProprietarioCarroResource.Id_Proprietario_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário possui as permissões para alterar a informação.
            switch (credencial.PerfilUsuario)
            {
                case TipoPerfil.ProprietarioCarro:
                    this.NotificarSeVerdadeiro(carro.Proprietario.IdUsuario != credencial.IdUsuario, CarroResource.Proprietario_Sem_Permissao_Alterar);
                    break;
            }

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);
            
            // Altera o carro
            carro.Alterar(entrada);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { CarroResource.Carro_Alterado_Com_Sucesso }, new CarroSaida(carro));
        }

        public async Task<ISaida> ExcluirCarro(int id, CredencialUsuarioEntrada credencial)
        {
            this.NotificarSeMenorOuIgualA(id, 0, CarroResource.Id_Carro_Nao_Existe);
            this.NotificarSeNulo(credencial, SharedResource.Credenciais_Usuario_Obrigatorias_Nao_Informadas);

            this.AdicionarNotificacoes(credencial?.Notificacoes);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var carro = await _carroRepositorio.ObterPorId(id);

            // Verifica se o carro existe
            this.NotificarSeNulo(carro, CarroResource.Id_Carro_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário possui as permissões para alterar a informação.
            switch (credencial.PerfilUsuario)
            {
                case TipoPerfil.ProprietarioCarro:
                    this.NotificarSeVerdadeiro(carro.Proprietario.IdUsuario != credencial.IdUsuario, CarroResource.Proprietario_Sem_Permissao_Excluir);
                    break;
            }

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Exclui o carro
            _carroRepositorio.Deletar(carro);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { CarroResource.Carro_Excluido_Com_Sucesso }, null);
        }
    }
}
