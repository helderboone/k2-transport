using Newtonsoft.Json;

namespace K2.Api.ViewModels
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados na alteração da senha do usuário
    /// </summary>
    public class AlterarSenhaUsuarioViewModel : BaseViewModel
    {
        public string SenhaAtual { get; set; }

        public string SenhaNova { get; set; }

        public string ConfirmacaoSenhaNova { get; set; }

        public bool EnviarEmailSenhaNova { get; set; }
    }
}
