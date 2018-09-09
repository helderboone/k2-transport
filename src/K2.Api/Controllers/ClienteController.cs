using K2.Api.ViewModels.ViewModels.Cliente;
using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace K2.Api.Controllers
{
    [Produces("application/json")]
    public class ClienteController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IClienteServico _clienteServico;

        public ClienteController(IClienteServico clienteServico, ILogger<ClienteController> logger)
        {
            _clienteServico = clienteServico;
            _logger = logger;
        }

        /// <summary>
        /// Realiza o cadastro de um novo cliente
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [Route("v1/clientes/cadastrar")]
        public async Task<ISaida> CadastrarCliente([FromBody]CadastrarClienteViewModel viewModel)
        {
            var entrada = new CadastrarClienteEntrada(
                viewModel?.Nome,
                viewModel?.Email,
                "k2",
                viewModel?.Cpf,
                viewModel?.Rg,
                viewModel?.Celular,
                viewModel?.Cep,
                viewModel?.Endereco,
                viewModel?.Municipio,
                viewModel?.Uf);

            return await _clienteServico.CadastrarCliente(entrada);
        }
    }
}