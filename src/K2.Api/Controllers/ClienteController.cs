using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace K2.Api.Controllers
{
    [Produces("application/json")]
    public class ClienteController : BaseController
    {
        private readonly IClienteServico _clienteServico;

        public ClienteController(IClienteServico clienteServico)
        {
            _clienteServico = clienteServico;
        }

        /// <summary>
        /// Realiza uma procura por clientes a partir dos parâmetros informados
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("v1/clientes/procurar")]
        public async Task<ISaida> Procurar([FromBody] ProcurarClienteEntrada model)
        {
            var entrada = new ProcurarClienteEntrada(model?.OrdenarPor, model?.OrdenarSentido, model?.PaginaIndex, model?.PaginaTamanho)
            {
                Nome  = model?.Nome,
                Email = model?.Email,
                Cpf   = model?.Cpf,
                Rg    = model?.Rg
            };

            return await _clienteServico.ProcurarClientes(entrada);
        }

        /// <summary>
        /// Realiza o cadastro de um novo cliente
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("v1/clientes/cadastrar")]
        public async Task<ISaida> Cadastrar([FromBody] CadastrarClienteEntrada model)
        {
            var entrada = new CadastrarClienteEntrada(
                model.Nome,
                model.Email,
                "k2",
                model.Cpf,
                model.Rg,
                model.Celular,
                model.Cep,
                model.Endereco,
                model.Municipio,
                model.Uf);

            return await _clienteServico.CadastrarCliente(entrada);
        }

        /// <summary>
        /// Realiza a alteração de um cliente
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPut]
        [Route("v1/clientes/alterar")]
        public async Task<ISaida> Alterar([FromBody] AlterarClienteEntrada model)
        {
            var entrada = new AlterarClienteEntrada(
                model.Id,
                model.Nome,
                model.Email,
                model.Cpf,
                model.Rg,
                model.Celular,
                model.Ativo,
                model.Cep,
                model.Endereco,
                model.Municipio,
                model.Uf);

            return await _clienteServico.AlterarCliente(entrada);
        }

        /// <summary>
        /// Obtém um cliente a partir do seu ID
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpGet]
        [Route("v1/clientes/obter-por-id/{id:int}")]
        public async Task<ISaida> ObterPorId(int id)
        {
            return await _clienteServico.ObterClientePorId(id);
        }

        /// <summary>
        /// Realiza a exclusão de um cliente.
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpDelete]
        [Route("v1/clientes/excluir/{id:int}")]
        public async Task<ISaida> ExcluirCliente(int id)
        {
            return await _clienteServico.ExcluirCliente(id);
        }
    }
}