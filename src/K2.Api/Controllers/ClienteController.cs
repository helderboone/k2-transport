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
        public async Task<ISaida> Procurar([FromBody] Models.ProcurarClienteEntrada model)
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
        public async Task<ISaida> CadastrarCliente([FromBody] Models.CadastrarClienteEntrada model)
        {
            var entrada = new CadastrarClienteEntrada(
                model?.Nome,
                model?.Email,
                "k2",
                model?.Cpf,
                model?.Rg,
                model?.Celular,
                model?.Cep,
                model?.Endereco,
                model?.Municipio,
                model?.Uf);

            return await _clienteServico.CadastrarCliente(entrada);
        }
    }
}