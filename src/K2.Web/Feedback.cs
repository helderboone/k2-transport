using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace K2.Web
{
    /// <summary>
    /// Representa um feedback exibido pelo sistema
    /// </summary>
    public class Feedback
    {
        [JsonProperty("Tipo")]
        public TipoFeedback Tipo { get; }

        [JsonProperty("Mensagem")]
        public string Mensagem { get; }

        [JsonProperty("MensagemAdicional")]
        public string MensagemAdicional { get; set; }

        [JsonProperty("TipoAcao")]
        public TipoAcaoAoOcultarFeedback TipoAcao { get; set; }

        public Feedback(TipoFeedback tipo, string mensagem, IEnumerable<string> mensagensAdicionais = null, TipoAcaoAoOcultarFeedback tipoAcao = TipoAcaoAoOcultarFeedback.Ocultar)
        {
            Tipo = tipo;
            Mensagem = mensagem;
            MensagemAdicional = mensagensAdicionais != null && mensagensAdicionais.Any()
                ? "<ul style=\"text-align:justify;\">" + string.Join(string.Empty, mensagensAdicionais.Where(x => !string.IsNullOrEmpty(x)).Select(x => "<li>" + x + "</li>")) + "</ul>"
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

    /// <summary>
    /// Tipo de ação que deverá ser executada ao ocultar o feedback
    /// </summary>
    public enum TipoAcaoAoOcultarFeedback
    {
        Ocultar = 0,
        /// <summary>
        /// Redireciona para a página anteriormente exibida antes da exibição da mensagem de erro
        /// </summary>
        VoltarPaginaAnterior = 1,
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