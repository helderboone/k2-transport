using K2.Api.ViewModels;
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
        /// Realiza uma procura por clientes a partir dos parâmetros informados
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("v1/clientes/procurar")]
        public async Task<ISaida> Procurar([FromBody] ProcurarClienteViewModel viewModel)
        {
            var procurarEntrada = new ProcurarClienteEntrada(viewModel?.OrdenarPor, viewModel?.OrdenarSentido, viewModel?.PaginaIndex, viewModel?.PaginaTamanho)
            {
                Nome  = viewModel?.Nome,
                Email = viewModel?.Email,
                Cpf   = viewModel?.Cpf,
                Rg    = viewModel?.Rg
            };

            return await _clienteServico.ProcurarClientes(procurarEntrada);
        }

        /// <summary>
        /// Realiza o cadastro de um novo cliente
        /// </summary>
        [Authorize(Policy = "Administrador")]
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