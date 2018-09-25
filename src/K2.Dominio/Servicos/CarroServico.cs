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
        private readonly IUow _uow;

        public CarroServico(ICarroRepositorio carroRepositorio, IUow uow)
        {
            _carroRepositorio = carroRepositorio;
            _uow              = uow;
        }

        public async Task<ISaida> ObterCarroPorId(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, CarroResource.Id_Carro_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var carro = await _carroRepositorio.ObterPorId(id);

            // Verifica se o carro existe
            this.NotificarSeNulo(carro, CarroResource.Id_Carro_Nao_Existe);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { CarroResource.Carro_Encontrado_Com_Sucesso }, new CarroSaida(carro));
        }

        public async Task<ISaida> ProcurarCarros(ProcurarCarroEntrada entrada)
        {
            // Verifica se os parâmetros para a procura foram informadas corretamente
            return entrada.Invalido
                ? new Saida(false, entrada.Mensagens, null)
                : await _carroRepositorio.Procurar(entrada);
        }

        public async Task<ISaida> CadastrarCarro(CadastrarCarroEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Cadastra o carro 
            var carro = new Carro(entrada);

            await _carroRepositorio.Inserir(carro);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { CarroResource.Carro_Cadastrado_Com_Sucesso }, new CarroSaida(carro));
        }

        public async Task<ISaida> AlterarCarro(AlterarCarroEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var carro = await _carroRepositorio.ObterPorId(entrada.Id, true);

            // Verifica se o carro existe
            this.NotificarSeNulo(carro, CarroResource.Id_Carro_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Altera o carro
            carro.Alterar(entrada);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { CarroResource.Carro_Alterado_Com_Sucesso }, new CarroSaida(carro));
        }

        public async Task<ISaida> ExcluirCarro(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, CarroResource.Id_Carro_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var carro = await _carroRepositorio.ObterPorId(id);

            // Verifica se o carro existe
            this.NotificarSeNulo(carro, CarroResource.Id_Carro_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _carroRepositorio.Deletar(carro);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { CarroResource.Carro_Excluido_Com_Sucesso }, null);
        }
    }
}
