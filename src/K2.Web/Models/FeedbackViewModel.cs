using System;

namespace K2.Web.Models
{
    /// <summary>
    /// Representa uma mensagem exibida pelo sistema
    /// </summary>
    public class FeedbackViewModel
    {
        public TipoFeedback Tipo { get; }

        public string Mensagem { get; }

        public string MensagemAdicional { get; set; }

        /// <summary>
        /// Tipo de ação que deverá ser realizada quando a mensagem é ocultada 
        /// </summary>
        public TipoAcaoOcultarFeedback TipoAcao { get; set; }

        public FeedbackViewModel(TipoFeedback tipo, string mensagem, string mensagemAdicional = null, TipoAcaoOcultarFeedback tipoAcao = TipoAcaoOcultarFeedback.FecharJanela)
        {
            Tipo = tipo;
            Mensagem = mensagem;
            MensagemAdicional = mensagemAdicional;
            TipoAcao = tipoAcao;
        }
    }

    public class FeedbackExceptionViewModel : FeedbackViewModel
    {
        public string ExceptionMessage { get; }

        public string StackTrace { get; }

        public FeedbackExceptionViewModel(Exception exception, string mensagem, string mensagemAdicional = null, TipoAcaoOcultarFeedback tipoAcao = TipoAcaoOcultarFeedback.FecharJanela)
            : base (TipoFeedback.ERRO, mensagem, mensagemAdicional, tipoAcao)
        {
            ExceptionMessage = exception.GetBaseException().Message;
            StackTrace = exception.StackTrace;
        }
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
