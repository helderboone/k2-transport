namespace K2.Api.ViewModels
{
    public class AlterarSenhaUsuarioViewModel
    {
        public string SenhaAtual { get; set; }

        public string SenhaNova { get; set; }

        public string ConfirmacaoSenhaNova { get; set; }

        public bool EnviarEmailSenhaNova { get; set; }
    }
}
