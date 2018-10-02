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
    public class LocalidadeServico : Notificavel, ILocalidadeServico
    {
        private readonly ILocalidadeRepositorio _localidadeRepositorio;
        private readonly IUow _uow;

        public LocalidadeServico(ILocalidadeRepositorio localidadeRepositorio, IUow uow)
        {
            _localidadeRepositorio = localidadeRepositorio;
            _uow                   = uow;
        }

        public async Task<ISaida> ObterLocalidadePorId(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, LocalidadeResource.Id_Localidade_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var localidade = await _localidadeRepositorio.ObterPorId(id);

            // Verifica se a localidade existe
            this.NotificarSeNulo(localidade, LocalidadeResource.Id_Localidade_Nao_Existe);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { LocalidadeResource.Localidade_Encontrada_Com_Sucesso }, new LocalidadeSaida(localidade));
        }

        public async Task<ISaida> ProcurarLocalidades(ProcurarLocalidadeEntrada entrada)
        {
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se os parâmetros para a procura foram informadas corretamente
            return entrada.Invalido
                ? new Saida(false, entrada.Mensagens, null)
                : await _localidadeRepositorio.Procurar(entrada);
        }

        public async Task<ISaida> CadastrarLocalidade(CadastrarLocalidadeEntrada entrada)
        {
            // Verifica se as informações para cadastro foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se já existe uma localidade com o mesmo nome e UF
            this.NotificarSeVerdadeiro(await _localidadeRepositorio.VerificarExistenciaPorNomeUf(entrada.Nome, entrada.Uf), LocalidadeResource.Localidade_Ja_Existe_Por_Nome_Uf);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Cadastra a localidade
            var localidade = new Localidade(entrada);

            await _localidadeRepositorio.Inserir(localidade);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { LocalidadeResource.Localidade_Cadastrada_Com_Sucesso }, new LocalidadeSaida(localidade));
        }

        public async Task<ISaida> AlterarLocalidade(AlterarLocalidadeEntrada entrada)
        {
            // Verifica se as informações para alteração foram informadas corretamente
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var localidade = await _localidadeRepositorio.ObterPorId(entrada.Id, true);

            // Verifica se a localidade existe
            this.NotificarSeNulo(localidade, LocalidadeResource.Id_Localidade_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se já existe uma localidade com o mesmo nome e UF
            this.NotificarSeVerdadeiro(await _localidadeRepositorio.VerificarExistenciaPorNomeUf(entrada.Nome, entrada.Uf, localidade.Id), LocalidadeResource.Localidade_Ja_Existe_Por_Nome_Uf);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Altera a localidade
            localidade.Alterar(entrada);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { LocalidadeResource.Localidade_Alterada_Com_Sucesso }, new LocalidadeSaida(localidade));
        }

        public async Task<ISaida> ExcluirLocalidade(int id)
        {
            this.NotificarSeMenorOuIgualA(id, 0, LocalidadeResource.Id_Localidade_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            var localidade = await _localidadeRepositorio.ObterPorId(id);

            // Verifica se a localidade existe
            this.NotificarSeNulo(localidade, LocalidadeResource.Id_Localidade_Nao_Existe);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            _localidadeRepositorio.Deletar(localidade);

            await _uow.Commit();

            return _uow.Invalido
                ? new Saida(false, _uow.Mensagens, null)
                : new Saida(true, new[] { LocalidadeResource.Localidade_Excluida_Com_Sucesso }, null);
        }
    }
}
