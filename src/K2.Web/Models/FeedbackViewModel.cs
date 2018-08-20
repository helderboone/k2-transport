using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
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

        public FeedbackViewModel(TipoFeedback tipo, string mensagem, string mensagemAdicional = null, TipoAcaoOcultarFeedback tipoAcao = TipoAcaoOcultarFeedback.FecharJanela)
        {
            Tipo = tipo;
            Mensagem = mensagem;
            MensagemAdicional = mensagemAdicional;
            TipoAcao = tipoAcao;
        }

        public FeedbackViewModel(TipoFeedback tipo, IEnumerable<string> mensagens, string mensagemAdicional = null, TipoAcaoOcultarFeedback tipoAcao = TipoAcaoOcultarFeedback.FecharJanela)
        {
            Tipo = tipo;
            Mensagem = string.Join(string.Empty, mensagens.Where(x => !string.IsNullOrEmpty(x)).Select(x => "<li>" + x + "</li>"));
            MensagemAdicional = mensagemAdicional;
            TipoAcao = tipoAcao;
        }
    }

    public class TipoFeedbackConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            TipoFeedback tipo = (TipoFeedback)value;

            writer.WriteRawValue("TipoFeedback." + tipo.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return Enum.Parse(typeof(TipoFeedback), (string)reader.Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TipoFeedback);
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
