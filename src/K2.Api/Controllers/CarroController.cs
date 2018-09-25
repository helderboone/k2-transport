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
        [Authorize(Policy = TipoPerfil.ProprietarioCarro)]
        [HttpPost]
        [Route("v1/carros/procurar")]
        public async Task<ISaida> Procurar([FromBody] Models.Entrada.ProcurarCarroEntrada model)
        {
            var entrada = new ProcurarCarroEntrada(
                base.ObterIdUsuario(),
                base.ObterPerfilUsuario(),
                model?.OrdenarPor,
                model?.OrdenarSentido,
                model?.PaginaIndex,
                model?.PaginaTamanho)
            {
                IdProprietario    = model?.IdProprietario,
                Descricao         = model?.Descricao,
                NomeFabricante    = model?.NomeFabricante,
                AnoModelo         = model?.AnoModelo,
                QuantidadeLugares = model?.QuantidadeLugares,
                Placa             = model?.Placa,
                Renavam           = model?.Renavam,
                Caracteristicas   = model?.Caracteristicas
            };

            return await _carroServico.ProcurarCarros(entrada);
        }

        /// <summary>
        /// Realiza o cadastro de um novo carro
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("v1/carros/cadastrar")]
        public async Task<ISaida> Cadastrar([FromBody] CadastrarCarroEntrada entrada)
        {
           return await _carroServico.CadastrarCarro(entrada);
        }

        /// <summary>
        /// Realiza a alteração de um carro
        /// </summary>
        [Authorize(Policy = TipoPerfil.ProprietarioCarro)]
        [HttpPut]
        [Route("v1/carros/alterar")]
        public async Task<ISaida> Alterar([FromBody] AlterarCarroEntrada entrada)
        {
            return await _carroServico.AlterarCarro(entrada);
        }

        /// <summary>
        /// Obtém um carro a partir do seu ID
        /// </summary>
        [Authorize(Policy = TipoPerfil.ProprietarioCarro)]
        [HttpGet]
        [Route("v1/carros/obter-por-id/{id:int}")]
        public async Task<ISaida> ObterPorId(int id)
        {
            return await _carroServico.ObterCarroPorId(id);
        }

        /// <summary>
        /// Realiza a exclusão de um carro.
        /// </summary>
        [Authorize(Policy = TipoPerfil.ProprietarioCarro)]
        [HttpDelete]
        [Route("v1/carros/excluir/{id:int}")]
        public async Task<ISaida> ExcluirCarro(int id)
        {
            return await _carroServico.ExcluirCarro(id);
        }
    }
}