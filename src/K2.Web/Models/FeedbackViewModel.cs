using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace K2.Web.Models
{
    /// <summary>
    /// Representa uma mensagem exibida pelo sistema
    /// </summary>
    public class FeedbackViewModel
    {
        [JsonProperty("Tipo")]
        public TipoFeedback Tipo { get; }

        [JsonProperty("Mensagem")]
        public string Mensagem { get; }

        [JsonProperty("MensagemAdicional")]
        public string MensagemAdicional { get; set; }

        [JsonProperty("TipoAcao")]
        public TipoAcaoOcultarFeedback TipoAcao { get; set; }

        public FeedbackViewModel(TipoFeedback tipo, string mensagem, IEnumerable<string> mensagensAdicionais = null, TipoAcaoOcultarFeedback tipoAcao = TipoAcaoOcultarFeedback.Ocultar)
        {
            Tipo = tipo;
            Mensagem = mensagem;
            MensagemAdicional = mensagensAdicionais != null && mensagensAdicionais.Any()
                ? string.Join(string.Empty, mensagensAdicionais.Where(x => !string.IsNullOrEmpty(x)).Select(x => "<li>" + x + "</li>"))
                : string.Empty; 
            TipoAcao = tipoAcao;
        }
    }

    /// <summary>
    /// Tipo de feedback
    /// </summary>
    public enum TipoFeedback
    {
        Info = 1,
        Atencao = 2,
        Erro = 3,
        Sucesso = 4
    }

    public enum TipoFeedbackResponse
    {
        Json,
        Html
    }

    /// <summary>
    /// Tipo de ação que deverá ser executada quando o botão para ocultar a mensagem de erro é ocultado
    /// </summary>
    public enum TipoAcaoOcultarFeedback
    {
        Ocultar = 0,
        /// <summary>
        /// Redireciona para a página anteriormente exibida antes da exibição da mensagem de erro
        /// </summary>
        RedirecionarPaginaAnterior = 1,
        /// <summary>
        /// Fecha a janela no caso de uma janela popup
        /// </summary>
        FecharJanela = 2,
        /// <summary>
        /// Redireciona para a página inicial
        /// </summary>
        RedirecionarTelaInicial = 3,
        /// <summary>
        /// Executa um reload na página (location.reload)
        /// </summary>
        ReloadPagina = 4,
        /// <summary>
        /// Oculta os popups abertos
        /// </summary>
        OcultarPopups = 5,
        /// <summary>
        /// Redireciona para tela de login
        /// </summary>
        RedirecionarTelaLogin = 6
    }
}
