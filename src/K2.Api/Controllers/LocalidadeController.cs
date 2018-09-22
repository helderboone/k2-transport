using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace K2.Api.Controllers
{
    [Produces("application/json")]
    public class LocalidadeController : BaseController
    {
        private readonly ILocalidadeServico _localidadeServico;

        public LocalidadeController(ILocalidadeServico localidadeServico)
        {
            _localidadeServico = localidadeServico;
        }

        /// <summary>
        /// Realiza uma procura por localidades a partir dos parâmetros informados
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("v1/localidades/procurar")]
        public async Task<ISaida> Procurar([FromBody] ProcurarLocalidadeEntrada model)
        {
            var entrada = new ProcurarLocalidadeEntrada(model?.OrdenarPor, model?.OrdenarSentido, model?.PaginaIndex, model?.PaginaTamanho)
            {
                Nome  = model?.Nome,
                Uf    = model?.Uf
            };

            return await _localidadeServico.ProcurarLocalidades(entrada);
        }

        /// <summary>
        /// Realiza o cadastro de uma nova localidade
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("v1/localidades/cadastrar")]
        public async Task<ISaida> Cadastrar([FromBody] CadastrarLocalidadeEntrada model)
        {
            var entrada = new CadastrarLocalidadeEntrada(
                model.Nome,
                model.Uf);

            return await _localidadeServico.CadastrarLocalidade(entrada);
        }

        /// <summary>
        /// Realiza a alteração de uma localidade
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPut]
        [Route("v1/localidades/alterar")]
        public async Task<ISaida> Alterar([FromBody] AlterarLocalidadeEntrada model)
        {
            var entrada = new AlterarLocalidadeEntrada(
                model.Id,
                model.Nome,
                model.Uf);

            return await _localidadeServico.AlterarLocalidade(entrada);
        }

        /// <summary>
        /// Obtém uma localidade a partir do seu ID
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpGet]
        [Route("v1/localidades/obter-por-id/{id:int}")]
        public async Task<ISaida> ObterPorId(int id)
        {
            return await _localidadeServico.ObterLocalidadePorId(id);
        }

        /// <summary>
        /// Realiza a exclusão de um localidade.
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpDelete]
        [Route("v1/localidades/excluir/{id:int}")]
        public async Task<ISaida> ExcluirLocalidade(int id)
        {
            return await _localidadeServico.ExcluirLocalidade(id);
        }
    }
}