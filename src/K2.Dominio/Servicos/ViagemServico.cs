using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Interfaces.Dados;
using K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios;
using K2.Dominio.Interfaces.Servicos;
using K2.Dominio.Resources;
using System.Linq;
using System.Threading.Tasks;

namespace K2.Dominio.Servicos
{
    public class ViagemServico : Notificavel, IViagemServico
    {
        private readonly IViagemRepositorio _viagemRepositorio;
        private readonly ICarroRepositorio _carroRepositorio;
        private readonly IMotoristaRepositorio _motoristaRepositorio;
        private readonly ILocalidadeRepositorio _localidadeRepositorio;
        private readonly IUow _uow;        

        public ViagemServico(
            IViagemRepositorio viagemRepositorio,
            ICarroRepositorio carroRepositorio,
            IMotoristaRepositorio motoristaRepositorio,
            ILocalidadeRepositorio localidadeRepositorio,
            IUow uow)
        {
            _viagemRepositorio     = viagemRepositorio;
            _carroRepositorio      = carroRepositorio;
            _motoristaRepositorio  = motoristaRepositorio;
            _localidadeRepositorio = localidadeRepositorio;
            _uow                   = uow;
        }

        public async Task<ISaida> ObterViagemPorId(int id, CredencialUsuarioEntrada credencial)
        {
            this.NotificarSeMenorOuIgualA(id, 0, ViagemResource.Id_Viagem_Nao_Existe);
            this.NotificarSeNulo(credencial, SharedResource.Credenciais_Usuario_Obrigatorias_Nao_Informadas);

            this.AdicionarNotificacoes(credencial?.Notificacoes);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var viagem = await _viagemRepositorio.ObterPorId(id);

            // Verifica se a viagem existe
            this.NotificarSeNulo(viagem, ViagemResource.Id_Viagem_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário possui as permissões para obter a informação.
            switch (credencial.PerfilUsuario)
            {
                case TipoPerfil.Motorista:
                    this.NotificarSeVerdadeiro(viagem.IdMotorista != credencial.IdUsuario, ViagemResource.Motorista_Sem_Permissao_Obter);
                    break;
                case TipoPerfil.ProprietarioCarro:
                    this.NotificarSeVerdadeiro(viagem.Carro.IdProprietario != credencial.IdUsuario, ViagemResource.Proprietario_Sem_Permissao_Obter);
                    break;
            }

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { ViagemResource.Viagem_Encontrada_Com_Sucesso }, new ViagemSaida(viagem));
        }

        public async Task<ISaida> ObterViagensPrevistas(CredencialUsuarioEntrada credencial)
        {
            this.NotificarSeNulo(credencial, SharedResource.Credenciais_Usuario_Obrigatorias_Nao_Informadas);

            this.AdicionarNotificacoes(credencial?.Notificacoes);

            var viagens = await _viagemRepositorio.ObterPrevistas(credencial);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { ViagemResource.Viagens_Encontradas_Com_Sucesso }, viagens.Select(x => new ViagemSaida(x)).ToList());
        }

        public async Task<ISaida> ObterViagensRealizadasOuCanceladas(CredencialUsuarioEntrada credencial)
        {
            this.NotificarSeNulo(credencial, SharedResource.Credenciais_Usuario_Obrigatorias_Nao_Informadas);

            this.AdicionarNotificacoes(credencial?.Notificacoes);

            var viagens = await _viagemRepositorio.ObterRealizadasOuCanceladas(credencial);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { ViagemResource.Viagens_Encontradas_Com_Sucesso }, viagens.Select(x => new ViagemSaida(x)).ToList());
        }

        public async Task<ISaida> ProcurarViagens(ProcurarViagemEntrada entrada, CredencialUsuarioEntrada credencial)
        {
            this.NotificarSeNulo(credencial, SharedResource.Credenciais_Usuario_Obrigatorias_Nao_Informadas);

            this.AdicionarNotificacoes(entrada?.Notificacoes);
            this.AdicionarNotificacoes(credencial?.Notificacoes);

            // Verifica se os parâmetros para a procura foram informadas corretamente
            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : await _viagemRepositorio.Procurar(entrada, credencial);
        }

        public async Task<ISaida> CadastrarViagem(CadastrarViagemEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se o carro com o ID informado existe
            this.NotificarSeFalso(await _carroRepositorio.VerificarExistenciaPorId(entrada.IdCarro), CarroResource.Id_Carro_Nao_Existe);

            // Verifica se o motorista com o ID informado existe
            this.NotificarSeFalso(await _motoristaRepositorio.VerificarExistenciaPorId(entrada.IdMotorista), MotoristaResource.Id_Motorista_Nao_Existe);

            // Verifica se a localidade de embarque com o ID informado existe
            this.NotificarSeFalso(await _localidadeRepositorio.VerificarExistenciaPorId(entrada.IdLocalidadeEmbarque), ViagemResource.Localidade_Embarque_Nao_Existe);

            // Verifica se a localidade de desembarque com o ID informado existe
            this.NotificarSeFalso(await _localidadeRepositorio.VerificarExistenciaPorId(entrada.IdLocalidadeDesembarque), ViagemResource.Localidade_Desembarque_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe uma viagem para o mesmo carro na data de saída
            this.NotificarSeVerdadeiro(await _viagemRepositorio.VerificarExistenciaPorCarroDataHorarioSaida(entrada.IdCarro, entrada.DataHorarioSaida), ViagemResource.Viagem_Ja_Existe_Para_Carro_Data_Saida);

            // Verifica se já existe uma viagem para o mesmo motorista na data de saída
            this.NotificarSeVerdadeiro(await _viagemRepositorio.VerificarExistenciaPorCarroDataHorarioSaida(entrada.IdMotorista, entrada.DataHorarioSaida), ViagemResource.Viagem_Ja_Existe_Para_Motorista_Data_Saida);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Cadastra a viagem
            var viagem = new Viagem(entrada);

            await _viagemRepositorio.Inserir(viagem);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ViagemResource.Viagem_Cadastrada_Com_Sucesso }, new ViagemSaida(await _viagemRepositorio.ObterPorId(viagem.Id)));
        }

        public async Task<ISaida> AlterarViagem(AlterarViagemEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var viagem = await _viagemRepositorio.ObterPorId(entrada.Id, true);

            // Verifica se a viagem existe
            this.NotificarSeNulo(viagem, ViagemResource.Id_Viagem_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o carro com o ID informado existe
            this.NotificarSeFalso(await _carroRepositorio.VerificarExistenciaPorId(entrada.IdCarro), CarroResource.Id_Carro_Nao_Existe);

            // Verifica se o motorista com o ID informado existe
            this.NotificarSeFalso(await _motoristaRepositorio.VerificarExistenciaPorId(entrada.IdMotorista), MotoristaResource.Id_Motorista_Nao_Existe);

            // Verifica se a localidade de embarque com o ID informado existe
            this.NotificarSeFalso(await _localidadeRepositorio.VerificarExistenciaPorId(entrada.IdLocalidadeEmbarque), ViagemResource.Localidade_Embarque_Nao_Existe);

            // Verifica se a localidade de desembarque com o ID informado existe
            this.NotificarSeFalso(await _localidadeRepositorio.VerificarExistenciaPorId(entrada.IdLocalidadeDesembarque), ViagemResource.Localidade_Desembarque_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe uma viagem para o mesmo carro na data de saída
            this.NotificarSeVerdadeiro(await _viagemRepositorio.VerificarExistenciaPorCarroDataHorarioSaida(entrada.IdCarro, entrada.DataHorarioSaida, viagem.Id), ViagemResource.Viagem_Ja_Existe_Para_Carro_Data_Saida);

            // Verifica se já existe uma viagem para o mesmo motorista na data de saída
            this.NotificarSeVerdadeiro(await _viagemRepositorio.VerificarExistenciaPorCarroDataHorarioSaida(entrada.IdMotorista, entrada.DataHorarioSaida, viagem.Id), ViagemResource.Viagem_Ja_Existe_Para_Motorista_Data_Saida);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Altera a viagem
            viagem.Alterar(entrada);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ViagemResource.Viagem_Alterada_Com_Sucesso }, new ViagemSaida(viagem));
        }

        public async Task<ISaida> ExcluirViagem(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, ViagemResource.Id_Viagem_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var viagem = await _viagemRepositorio.ObterPorId(id);

            // Verifica se a viagem existe
            this.NotificarSeNulo(viagem, ViagemResource.Id_Viagem_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _viagemRepositorio.Deletar(viagem);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ViagemResource.Viagem_Excluida_Com_Sucesso }, null);
        }
    }
}
