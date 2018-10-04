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
    public class ViagemController : BaseController
    {
        private readonly IViagemServico _viagemServico;

        public ViagemController(IViagemServico viagemServico)
        {
            _viagemServico = viagemServico;
        }

        /// <summary>
        /// Obtém uma viagem a partir do seu ID
        /// </summary>
        [Authorize(Policy = "MotoristaOuProprietarioCarro")]
        [HttpGet]
        [Route("v1/viagens/obter-por-id/{id:int}")]
        public async Task<ISaida> ObterPorId(int id)
        {
            var credencial = new CredencialUsuarioEntrada(base.ObterIdUsuario(), base.ObterPerfilUsuario());

            return await _viagemServico.ObterViagemPorId(id, credencial);
        }

        /// <summary>
        /// Obtém as viagens ainda previstas
        /// </summary>
        [Authorize(Policy = "MotoristaOuProprietarioCarro")]
        [HttpGet]
        [Route("v1/viagens/obter-previstas")]
        public async Task<ISaida> ObterPrevistas()
        {
            var credencial = new CredencialUsuarioEntrada(base.ObterIdUsuario(), base.ObterPerfilUsuario());

            return await _viagemServico.ObterViagensPrevistas(credencial);
        }

        /// <summary>
        /// Obtém as viagens realizadas ou canceladas
        /// </summary>
        [Authorize(Policy = "MotoristaOuProprietarioCarro")]
        [HttpGet]
        [Route("v1/viagens/obter-realizadas-ou-canceladas")]
        public async Task<ISaida> ObterRealizadasOuCanceladas()
        {
            var credencial = new CredencialUsuarioEntrada(base.ObterIdUsuario(), base.ObterPerfilUsuario());

            return await _viagemServico.ObterViagensRealizadasOuCanceladas(credencial);
        }

        /// <summary>
        /// Realiza uma procura por viagens a partir dos parâmetros informados
        /// </summary>
        [Authorize(Policy = "MotoristaOuProprietarioCarro")]
        [HttpPost]
        [Route("v1/viagens/procurar")]
        public async Task<ISaida> Procurar([FromBody] ProcurarViagemEntrada entrada)
        {
            var credencial = new CredencialUsuarioEntrada(base.ObterIdUsuario(), base.ObterPerfilUsuario());

            return await _viagemServico.ProcurarViagens(entrada, credencial);
        }

        /// <summary>
        /// Realiza o cadastro de uma nova viagem
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPost]
        [Route("v1/viagens/cadastrar")]
        public async Task<ISaida> Cadastrar([FromBody] CadastrarViagemEntrada entrada)
        {
            return await _viagemServico.CadastrarViagem(entrada);
        }

        /// <summary>
        /// Realiza a alteração de uma viagem
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpPut]
        [Route("v1/viagens/alterar")]
        public async Task<ISaida> Alterar([FromBody] AlterarViagemEntrada entrada)
        {
            return await _viagemServico.AlterarViagem(entrada);
        }

        /// <summary>
        /// Realiza a exclusão de um viagem.
        /// </summary>
        [Authorize(Policy = TipoPerfil.Administrador)]
        [HttpDelete]
        [Route("v1/viagens/excluir/{id:int}")]
        public async Task<ISaida> ExcluirViagem(int id)
        {
            return await _viagemServico.ExcluirViagem(id);
        }
    }
}