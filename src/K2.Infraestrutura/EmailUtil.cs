using JNogueira.Infraestrutura.Utilzao;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace K2.Infraestrutura
{
    public class EmailUtil
    {
        private readonly string _servidor;
        private readonly string _porta;
        private readonly string _usuarioSmtp;
        private readonly string _senhaSmtp;

        public EmailUtil(string servidor, string porta, string usuarioSmtp, string senhaSmtp)
        {
            _servidor = servidor;
            _porta = porta;
            _usuarioSmtp = usuarioSmtp;
            _senhaSmtp = senhaSmtp;
        }

        public void EnviarEmail(string emailRemetente, ICollection<string> emailDestinatarios, string assunto, string mensagem, string nomeRemetente = "K2 Transport")
        {
            var smtpClient = new SmtpClient(_servidor, Convert.ToInt32(_porta)) {
                Credentials = new NetworkCredential(_usuarioSmtp, _senhaSmtp),
                Timeout = 10000
            };

            var smtpUtil = new SmtpUtil(emailRemetente, emailDestinatarios, mensagem, smtpClient)
            {
                Assunto = assunto,
                NomeRemetente = nomeRemetente,
                MensagemEmHtml = true
            };

            smtpUtil.Enviar();
        }
    }
}
