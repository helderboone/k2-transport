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
    public class LogController : BaseController
    {
        private readonly ILogServico _logServico;

        public LogController(ILogServico logServico)
        {
            _logServico = logServico;
        }

        /// <summary>
        /// Realiza uma procura por registros no log a partir dos parâmetros informados
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.AnalistaTI)]
        [HttpPost]
        [Route("v1/log/procurar")]
        public async Task<ISaida> Procurar([FromBody] ProcurarLogEntrada entrada)
        {
            return await _logServico.ProcurarRegistros(entrada);
        }

        /// <summary>
        /// Obtém um registro do log a partir do seu ID
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.AnalistaTI)]
        [HttpGet]
        [Route("v1/log/obter-por-id/{id:int}")]
        public async Task<ISaida> ObterPorId(int id)
        {
            return await _logServico.ObterRegistroPorId(id);
        }

        /// <summary>
        /// Realiza a exclusão de um registro do log.
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.AnalistaTI)]
        [HttpDelete]
        [Route("v1/log/excluir/{id:int:min(1)}")]
        public async Task Excluir(int id)
        {
            await _logServico.ExcluirRegistro(id);
        }

        /// <summary>
        /// Realiza a exclusão de todos os registros do log.
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.AnalistaTI)]
        [HttpDelete]
        [Route("v1/log/limpar")]
        public async Task Limpar()
        {
            await _logServico.LimparLog();
        }
    }
}