namespace K2.Web.Models
{
    /// <summary>
    /// Representa uma mensagem exibida pelo sistema
    /// </summary>
    public class FeedbackViewModel
    {
        public FeedbackViewModel()
        {
            Tipo = TipoFeedback.INFO;
            TipoAcao = TipoAcaoOcultarFeedback.OcultarPopups;
        }

        public string Titulo { get; set; }

        public string Mensagem { get; set; }

        public string MensagemAdicional { get; set; }

        public TipoFeedback Tipo { get; set; }

        /// <summary>
        /// Tipo de ação que deverá ser realizada quando a mensagem é ocultada 
        /// </summary>
        public TipoAcaoOcultarFeedback TipoAcao { get; set; }
    }

    /// <summary>
    /// Tipo da mensagem
    /// </summary>
    public enum TipoFeedback
    {
        INFO = 1,
        ATENCAO = 2,
        ERRO = 3,
        SUCESSO = 4
    }

    public enum TipoResponseFeedback
    {
        Json,
        Html
    }

    /// <summary>
    /// Tipo de ação que deverá ser executada quando o botão para ocultar a mensagem de erro é ocultado
    /// </summary>
    public enum TipoAcaoOcultarFeedback
    {
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
