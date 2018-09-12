using System.Collections.Generic;

namespace K2.Dominio.Interfaces.Infraestrutura
{
    public interface IEmailHelper
    {
        /// <summary>
        /// Realiza o envio de e-mails
        /// </summary>
        void EnviarEmail(string emailRemetente, ICollection<string> emailDestinatarios, string assunto, string mensagem, string nomeRemetente = "K2 Transport");
    }
}
