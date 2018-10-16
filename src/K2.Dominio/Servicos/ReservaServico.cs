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
    public class ReservaServico : Notificavel, IReservaServico
    {
        private readonly IReservaRepositorio _reservaRepositorio;
        private readonly IReservaDependenteRepositorio _reservaDependenteRepositorio;
        private readonly IViagemRepositorio _viagemRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IUow _uow; 

        public ReservaServico(
            IReservaRepositorio reservaRepositorio,
            IReservaDependenteRepositorio reservaDependenteRepositorio,
            IViagemRepositorio viagemRepositorio,
            IClienteRepositorio clienteRepositorio,
            IUow uow)
        {
            _reservaRepositorio           = reservaRepositorio;
            _reservaDependenteRepositorio = reservaDependenteRepositorio;
            _viagemRepositorio            = viagemRepositorio;
            _clienteRepositorio           = clienteRepositorio;
            _uow                          = uow;
        }

        public async Task<ISaida> ObterReservaPorId(int id, CredencialUsuarioEntrada credencial)
        {
            this.NotificarSeMenorOuIgualA(id, 0, ReservaResource.Id_Reserva_Nao_Existe);
            this.NotificarSeNulo(credencial, SharedResource.Credenciais_Usuario_Obrigatorias_Nao_Informadas);

            this.AdicionarNotificacoes(credencial?.Notificacoes);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var reserva = await _reservaRepositorio.ObterPorId(id);

            // Verifica se a reserva existe
            this.NotificarSeNulo(reserva, ReservaResource.Id_Reserva_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário possui as permissões para obter a informação.
            switch (credencial.PerfilUsuario)
            {
                case TipoPerfil.Motorista:
                    this.NotificarSeVerdadeiro(reserva.Viagem.IdMotorista != credencial.IdUsuario, ReservaResource.Motorista_Sem_Permissao_Obter);
                    break;
                case TipoPerfil.ProprietarioCarro:
                    this.NotificarSeVerdadeiro(reserva.Viagem.Carro.IdProprietario != credencial.IdUsuario, ReservaResource.Proprietario_Sem_Permissao_Obter);
                    break;
            }

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { ReservaResource.Reserva_Encontrada_Com_Sucesso }, new ReservaSaida(reserva));
        }

        public async Task<ISaida> ObterReservasPorViagem(int idViagem, CredencialUsuarioEntrada credencial)
        {
            this.NotificarSeMenorOuIgualA(idViagem, 0, ViagemResource.Id_Viagem_Nao_Existe);
            this.NotificarSeNulo(credencial, SharedResource.Credenciais_Usuario_Obrigatorias_Nao_Informadas);

            this.AdicionarNotificacoes(credencial?.Notificacoes);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var viagem = await _viagemRepositorio.ObterPorId(idViagem);
            
            // Verifica se a viagem existe
            this.NotificarSeNulo(viagem, ViagemResource.Id_Viagem_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário possui as permissões para obter a informação.
            switch (credencial.PerfilUsuario)
            {
                case TipoPerfil.Motorista:
                    this.NotificarSeVerdadeiro(viagem.IdMotorista != credencial.IdUsuario, ReservaResource.Motorista_Sem_Permissao_Obter);
                    break;
                case TipoPerfil.ProprietarioCarro:
                    this.NotificarSeVerdadeiro(viagem.Carro.IdProprietario != credencial.IdUsuario, ReservaResource.Proprietario_Sem_Permissao_Obter);
                    break;
            }

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { ReservaResource.Reservas_Viagem_Encontradas_Com_Sucesso }, (await _reservaRepositorio.ObterPorIdVigem(idViagem)).Select(x => new ReservaSaida(x)).ToList());
        }

        public async Task<ISaida> ProcurarReservas(ProcurarReservaEntrada entrada, CredencialUsuarioEntrada credencial)
        {
            this.NotificarSeNulo(credencial, SharedResource.Credenciais_Usuario_Obrigatorias_Nao_Informadas);

            this.AdicionarNotificacoes(entrada?.Notificacoes);
            this.AdicionarNotificacoes(credencial?.Notificacoes);

            // Verifica se os parâmetros para a procura foram informadas corretamente
            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : await _reservaRepositorio.Procurar(entrada, credencial);
        }

        public async Task<ISaida> CadastrarReserva(CadastrarReservaEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se o cliente com o ID informado existe
            this.NotificarSeFalso(await _clienteRepositorio.VerificarExistenciaPorId(entrada.IdCliente), ClienteResource.Id_Cliente_Nao_Existe);

            // Verifica se a viagem com o ID informado existe
            this.NotificarSeFalso(await _viagemRepositorio.VerificarExistenciaPorId(entrada.IdViagem), ViagemResource.Id_Viagem_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe uma reserva para a mesma viagem e cliente
            this.NotificarSeVerdadeiro(await _reservaRepositorio.VerificarExistenciaPorClienteViagem(entrada.IdCliente, entrada.IdViagem), ReservaResource.Reserva_Ja_Existe_Para_Cliente_Viagem);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Cadastra a reserva
            var reserva = new Reserva(entrada);

            await _reservaRepositorio.Inserir(reserva);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ReservaResource.Reserva_Cadastrada_Com_Sucesso }, new ReservaSaida(await _reservaRepositorio.ObterPorId(reserva.Id)));
        }

        public async Task<ISaida> AlterarReserva(AlterarReservaEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var reserva = await _reservaRepositorio.ObterPorId(entrada.Id, true);

            // Verifica se a reserva existe
            this.NotificarSeNulo(reserva, ReservaResource.Id_Reserva_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o cliente com o ID informado existe
            this.NotificarSeFalso(await _clienteRepositorio.VerificarExistenciaPorId(entrada.IdCliente), ClienteResource.Id_Cliente_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe uma reserva para a mesma viagem e cliente
            this.NotificarSeVerdadeiro(await _reservaRepositorio.VerificarExistenciaPorClienteViagem(entrada.IdCliente, reserva.IdViagem, reserva.Id), ReservaResource.Reserva_Ja_Existe_Para_Cliente_Viagem);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Altera a reserva
            reserva.Alterar(entrada);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ReservaResource.Reserva_Alterada_Com_Sucesso }, new ReservaSaida(await _reservaRepositorio.ObterPorId(reserva.Id)));
        }

        public async Task<ISaida> ExcluirReserva(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, ReservaResource.Id_Reserva_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var reserva = await _reservaRepositorio.ObterPorId(id);

            // Verifica se a reserva existe
            this.NotificarSeNulo(reserva, ReservaResource.Id_Reserva_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            if (reserva.Dependente != null)
                _reservaDependenteRepositorio.Deletar(reserva.Dependente);

            _reservaRepositorio.Deletar(reserva);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ReservaResource.Reserva_Excluida_Com_Sucesso }, null);
        }

        public async Task<ISaida> ObterDependentePorReserva(int idReserva)
        {
            this.NotificarSeMenorOuIgualA(idReserva, 0, ReservaResource.Id_Reserva_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var dependente = await _reservaDependenteRepositorio.ObterPorIdReserva(idReserva);

            // Verifica se o dependente existe
            this.NotificarSeNulo(dependente, ReservaDependenteResource.Dependente_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { ReservaDependenteResource.Dependente_Encontrado_Com_Sucesso }, new ReservaDependenteSaida(dependente));
        }

        public async Task<ISaida> CadastrarDependente(CadastrarReservaDependenteEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se a reserva com o ID informado existe
            this.NotificarSeFalso(await _reservaRepositorio.VerificarExistenciaPorId(entrada.IdReserva), ReservaResource.Id_Reserva_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe um dependente para a reserva
            this.NotificarSeVerdadeiro(await _reservaDependenteRepositorio.VerificarExistenciaPorReserva(entrada.IdReserva), ReservaDependenteResource.Somente_Um_Dependente_Permitido_Por_Reserva);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Cadastra o dependente
            var dependente = new ReservaDependente(entrada);

            await _reservaDependenteRepositorio.Inserir(dependente);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ReservaDependenteResource.Dependente_Cadastrado_Com_Sucesso }, new ReservaDependenteSaida(await _reservaDependenteRepositorio.ObterPorIdReserva(dependente.IdReserva)));
        }

        public async Task<ISaida> AlterarDependente(AlterarReservaDependenteEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var dependente = await _reservaDependenteRepositorio.ObterPorIdReserva(entrada.IdReserva, true);

            // Verifica se o dependente existe
            this.NotificarSeNulo(dependente, ReservaDependenteResource.Dependente_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Altera o dependente
            dependente.Alterar(entrada);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ReservaDependenteResource.Dependente_Alterado_Com_Sucesso }, new ReservaDependenteSaida(dependente));
        }

        public async Task<ISaida> ExcluirDependente(int idReserva)
        {
            this.NotificarSeMenorOuIgualA(idReserva, 0, ReservaResource.Id_Reserva_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var dependente = await _reservaDependenteRepositorio.ObterPorIdReserva(idReserva);

            // Verifica se o dependente existe
            this.NotificarSeNulo(dependente, ReservaDependenteResource.Dependente_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _reservaDependenteRepositorio.Deletar(dependente);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { ReservaDependenteResource.Dependente_Excluido_Com_Sucesso }, null);
        }
    }
}
