namespace K2.Api.ViewModels
{
    public class ProcurarClienteViewModel : ProcurarViewModel
    {
        public string Nome { get; set; }

        public string Email { get; set; }

        public string Cpf { get; set; }

        public string Rg { get; set; }


        public ProcurarClienteViewModel()
        {
            this.OrdenarPor = "Nome";
        }
    }
}
