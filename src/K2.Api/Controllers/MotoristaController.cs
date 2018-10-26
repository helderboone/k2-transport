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
    public class MotoristaController : BaseController
    {
        private readonly IMotoristaServico _motoristaServico;

        public MotoristaController(IMotoristaServico motoristaServico)
        {
            _motoristaServico = motoristaServico;
        }

        /// <summary>
        /// Realiza uma procura por motoristas a partir dos parâmetros informados
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.MotoristaOuProprietarioCarro)]
        [HttpPost]
        [Route("v1/motoristas/procurar")]
        public async Task<ISaida> Procurar([FromBody] ProcurarMotoristaEntrada entrada)
        {
            return await _motoristaServico.ProcurarMotoristas(entrada);
        }

        /// <summary>
        /// Realiza o cadastro de um novo motorista
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("v1/motoristas/cadastrar")]
        public async Task<ISaida> Cadastrar([FromBody] CadastrarMotoristaEntrada entrada)
        {
            return await _motoristaServico.CadastrarMotorista(entrada);
        }

        /// <summary>
        /// Realiza a alteração de um motorista
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPut]
        [Route("v1/motoristas/alterar")]
        public async Task<ISaida> Alterar([FromBody] AlterarMotoristaEntrada entrada)
        {
            return await _motoristaServico.AlterarMotorista(entrada);
        }

        /// <summary>
        /// Obtém um motorista a partir do seu ID
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpGet]
        [Route("v1/motoristas/obter-por-id/{id:int}")]
        public async Task<ISaida> ObterPorId(int id)
        {
            return await _motoristaServico.ObterMotoristaPorId(id);
        }

        /// <summary>
        /// Obtém um motorista a partir do ID do seu usuário
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.MotoristaOuProprietarioCarro)]
        [HttpGet]
        [Route("v1/motoristas/obter-por-id-usuario/{idUsuario:int}")]
        public async Task<ISaida> ObterPorIdUsuario(int idUsuario)
        {
            return await _motoristaServico.ObterMotoristaPorIdUsuario(idUsuario);
        }

        /// <summary>
        /// Realiza a exclusão de um motorista.
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpDelete]
        [Route("v1/motoristas/excluir/{id:int}")]
        public async Task<ISaida> ExcluirMotorista(int id)
        {
            return await _motoristaServico.ExcluirMotorista(id);
        }
    }
}