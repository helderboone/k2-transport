using K2.Dominio;
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
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("v1/localidades/procurar")]
        public async Task<ISaida> Procurar([FromBody] ProcurarLocalidadeEntrada entrada)
        {
            return await _localidadeServico.ProcurarLocalidades(entrada);
        }

        /// <summary>
        /// Realiza o cadastro de uma nova localidade
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("v1/localidades/cadastrar")]
        public async Task<ISaida> Cadastrar([FromBody] CadastrarLocalidadeEntrada entrada)
        {
            return await _localidadeServico.CadastrarLocalidade(entrada);
        }

        /// <summary>
        /// Realiza a alteração de uma localidade
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPut]
        [Route("v1/localidades/alterar")]
        public async Task<ISaida> Alterar([FromBody] AlterarLocalidadeEntrada entrada)
        {
            return await _localidadeServico.AlterarLocalidade(entrada);
        }

        /// <summary>
        /// Obtém uma localidade a partir do seu ID
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpGet]
        [Route("v1/localidades/obter-por-id/{id:int}")]
        public async Task<ISaida> ObterPorId(int id)
        {
            return await _localidadeServico.ObterLocalidadePorId(id);
        }

        /// <summary>
        /// Realiza a exclusão de um localidade.
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpDelete]
        [Route("v1/localidades/excluir/{id:int}")]
        public async Task<ISaida> ExcluirLocalidade(int id)
        {
            return await _localidadeServico.ExcluirLocalidade(id);
        }
    }
}