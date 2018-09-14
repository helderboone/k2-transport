using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
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
        public async Task<Models.ProcurarSaida> Procurar([FromBody] Models.ProcurarClienteEntrada model)
        {
            var entrada = new ProcurarClienteEntrada(model?.OrdenarPor, model?.OrdenarSentido, model?.PaginaIndex, model?.PaginaTamanho)
            {
                Nome  = model?.Nome,
                Email = model?.Email,
                Cpf   = model?.Cpf,
                Rg    = model?.Rg
            };

            var saida = await _clienteServico.ProcurarClientes(entrada);

            var retorno = (ProcurarRetorno)saida.Retorno;

            return new Models.ProcurarSaida(saida.Sucesso, saida.Mensagens, new Models.ProcurarRetorno(
                retorno.PaginaIndex,
                retorno.PaginaTamanho,
                retorno.OrdenarPor,
                retorno.OrdenarSentido,
                retorno.TotalRegistros,
                retorno.TotalPaginas,
                retorno.Registros));
        }

        /// <summary>
        /// Realiza o cadastro de um novo cliente
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPost]
        [Route("v1/clientes/cadastrar")]
        public async Task<Models.Saida> Cadastrar([FromBody] Models.CadastrarClienteEntrada model)
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

            var saida = await _clienteServico.CadastrarCliente(entrada);

            return new Models.Saida(saida.Sucesso, saida.Mensagens, saida.Retorno);
        }

        /// <summary>
        /// Realiza a alteração de um cliente
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpPut]
        [Route("v1/clientes/alterar")]
        public async Task<Models.Saida> Alterar([FromBody] Models.AlterarClienteEntrada model)
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

            var saida = await _clienteServico.AlterarCliente(entrada);

            return new Models.Saida(saida.Sucesso, saida.Mensagens, saida.Retorno);
        }

        /// <summary>
        /// Obtém um cliente a partir do seu ID
        /// </summary>
        [Authorize(Policy = "Administrador")]
        [HttpGet]
        [Route("v1/clientes/obter-por-id/{id:int}")]
        public async Task<Models.Saida> ObterPorId(int id)
        {
            var saida =  await _clienteServico.ObterClientePorId(id);

            return new Models.Saida(saida.Sucesso, saida.Mensagens, saida.Retorno);
        }
    }
}