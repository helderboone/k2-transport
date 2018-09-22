using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace K2.Api.Controllers
{
    [Produces("application/json")]
    public class ProprietarioCarroController : BaseController
    {
        private readonly IProprietarioCarroServico _proprietarioCarroServico;

        public ProprietarioCarroController(IProprietarioCarroServico proprietarioCarroServico)
        {
            _proprietarioCarroServico = proprietarioCarroServico;
        }

        /// <summary>
        /// Realiza uma procura por proprietários a partir dos parâmetros informados
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("v1/proprietarios-carro/procurar")]
        public async Task<ISaida> Procurar([FromBody] ProcurarProprietarioCarroEntrada model)
        {
            var entrada = new ProcurarProprietarioCarroEntrada(model?.OrdenarPor, model?.OrdenarSentido, model?.PaginaIndex, model?.PaginaTamanho)
            {
                Nome  = model?.Nome,
                Email = model?.Email,
                Cpf   = model?.Cpf,
                Rg    = model?.Rg
            };

            return await _proprietarioCarroServico.ProcurarProprietarioCarros(entrada);
        }

        /// <summary>
        /// Realiza o cadastro de um novo proprietário
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("v1/proprietarios-carro/cadastrar")]
        public async Task<ISaida> Cadastrar([FromBody] CadastrarProprietarioCarroEntrada model)
        {
            var entrada = new CadastrarProprietarioCarroEntrada(
                model.Nome,
                model.Email,
                "k2",
                model.Cpf,
                model.Rg,
                model.Celular);

            return await _proprietarioCarroServico.CadastrarProprietarioCarro(entrada);
        }

        /// <summary>
        /// Realiza a alteração de um proprietário
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPut]
        [Route("v1/proprietarios-carro/alterar")]
        public async Task<ISaida> Alterar([FromBody] AlterarProprietarioCarroEntrada model)
        {
            var entrada = new AlterarProprietarioCarroEntrada(
                model.Id,
                model.Nome,
                model.Email,
                model.Cpf,
                model.Rg,
                model.Celular,
                model.Ativo);

            return await _proprietarioCarroServico.AlterarProprietarioCarro(entrada);
        }

        /// <summary>
        /// Obtém um proprietário a partir do seu ID
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpGet]
        [Route("v1/proprietarios-carro/obter-por-id/{id:int}")]
        public async Task<ISaida> ObterPorId(int id)
        {
            return await _proprietarioCarroServico.ObterProprietarioCarroPorId(id);
        }

        /// <summary>
        /// Realiza a exclusão de um proprietário.
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpDelete]
        [Route("v1/proprietarios-carro/excluir/{id:int}")]
        public async Task<ISaida> ExcluirProprietarioCarro(int id)
        {
            return await _proprietarioCarroServico.ExcluirProprietarioCarro(id);
        }
    }
}