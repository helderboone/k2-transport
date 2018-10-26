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
    public class CarroController : BaseController
    {
        private readonly ICarroServico _carroServico;

        public CarroController(ICarroServico carroServico)
        {
            _carroServico = carroServico;
        }

        /// <summary>
        /// Realiza uma procura por carros a partir dos parâmetros informados
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.MotoristaOuProprietarioCarro)]
        [HttpPost]
        [Route("v1/carros/procurar")]
        public async Task<ISaida> Procurar([FromBody] ProcurarCarroEntrada entrada)
        {
            var credencial = new CredencialUsuarioEntrada(base.ObterIdUsuario(), base.ObterPerfilUsuario());

            return await _carroServico.ProcurarCarros(entrada, credencial);
        }

        /// <summary>
        /// Realiza o cadastro de um novo carro
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.Administrador)]
        [HttpPost]
        [Route("v1/carros/cadastrar")]
        public async Task<ISaida> Cadastrar([FromBody] CadastrarCarroEntrada entrada)
        {
            return await _carroServico.CadastrarCarro(entrada);
        }

        /// <summary>
        /// Realiza a alteração de um carro
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.ProprietarioCarro)]
        [HttpPut]
        [Route("v1/carros/alterar")]
        public async Task<ISaida> Alterar([FromBody] AlterarCarroEntrada entrada)
        {
            var credencial = new CredencialUsuarioEntrada(base.ObterIdUsuario(), base.ObterPerfilUsuario());

            return await _carroServico.AlterarCarro(entrada, credencial);
        }

        /// <summary>
        /// Obtém um carro a partir do seu ID
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.ProprietarioCarro)]
        [HttpGet]
        [Route("v1/carros/obter-por-id/{id:int}")]
        public async Task<ISaida> ObterPorId(int id)
        {
            var credencial = new CredencialUsuarioEntrada(base.ObterIdUsuario(), base.ObterPerfilUsuario());

            return await _carroServico.ObterCarroPorId(id, credencial);
        }

        /// <summary>
        /// Realiza a exclusão de um carro.
        /// </summary>
        [Authorize(Policy = TipoPoliticaAcesso.ProprietarioCarro)]
        [HttpDelete]
        [Route("v1/carros/excluir/{id:int}")]
        public async Task<ISaida> ExcluirCarro(int id)
        {
            var credencial = new CredencialUsuarioEntrada(base.ObterIdUsuario(), base.ObterPerfilUsuario());

            return await _carroServico.ExcluirCarro(id, credencial);
        }
    }
}