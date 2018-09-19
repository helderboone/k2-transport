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
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("v1/motoristas/procurar")]
        public async Task<ISaida> Procurar([FromBody] ProcurarMotoristaEntrada model)
        {
            var entrada = new ProcurarMotoristaEntrada(model?.OrdenarPor, model?.OrdenarSentido, model?.PaginaIndex, model?.PaginaTamanho)
            {
                Nome  = model?.Nome,
                Email = model?.Email,
                Cpf   = model?.Cpf,
                Rg    = model?.Rg,
                Cnh   = model?.Cnh
            };

            return await _motoristaServico.ProcurarMotoristas(entrada);
        }

        /// <summary>
        /// Realiza o cadastro de um novo motorista
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("v1/motoristas/cadastrar")]
        public async Task<ISaida> Cadastrar([FromBody] CadastrarMotoristaEntrada model)
        {
            var entrada = new CadastrarMotoristaEntrada(
                model.Nome,
                model.Email,
                "k2",
                model.Cpf,
                model.Rg,
                model.Celular,
                model.Cnh);

            return await _motoristaServico.CadastrarMotorista(entrada);
        }

        /// <summary>
        /// Realiza a alteração de um motorista
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPut]
        [Route("v1/motoristas/alterar")]
        public async Task<ISaida> Alterar([FromBody] AlterarMotoristaEntrada model)
        {
            var entrada = new AlterarMotoristaEntrada(
                model.Id,
                model.Nome,
                model.Email,
                model.Cpf,
                model.Rg,
                model.Celular,
                model.Ativo,
                model.Cnh);

            return await _motoristaServico.AlterarMotorista(entrada);
        }

        /// <summary>
        /// Obtém um motorista a partir do seu ID
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpGet]
        [Route("v1/motoristas/obter-por-id/{id:int}")]
        public async Task<ISaida> ObterPorId(int id)
        {
            return await _motoristaServico.ObterMotoristaPorId(id);
        }

        /// <summary>
        /// Realiza a exclusão de um motorista.
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpDelete]
        [Route("v1/motoristas/excluir/{id:int}")]
        public async Task<ISaida> ExcluirMotorista(int id)
        {
            return await _motoristaServico.ExcluirMotorista(id);
        }
    }
}