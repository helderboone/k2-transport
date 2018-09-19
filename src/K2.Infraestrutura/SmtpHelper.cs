using JNogueira.Infraestrutura.Utilzao;
using K2.Dominio.Interfaces.Infraestrutura;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;

namespace K2.Infraestrutura
{
    public class SmtpHelper : IEmailHelper
    {
        private readonly string _servidor;
        private readonly string _porta;
        private readonly string _usuarioSmtp;
        private readonly string _senhaSmtp;

        public SmtpHelper(string servidor, string porta, string usuarioSmtp, string senhaSmtp)
        {
            _servidor = servidor;
            _porta = porta;
            _usuarioSmtp = usuarioSmtp;
            _senhaSmtp = senhaSmtp;
        }

        public void EnviarEmail(ICollection<string> emailDestinatarios, string assunto, string mensagem, string nomeRemetente = "K2 Transport")
        {
            var smtpClient = new SmtpClient(_servidor, Convert.ToInt32(_porta)) {
                Credentials = new NetworkCredential(_usuarioSmtp, _senhaSmtp),
                Timeout = 10000
            };

            var smtpUtil = new SmtpUtil(_usuarioSmtp, emailDestinatarios, mensagem, smtpClient)
            {
                Assunto = assunto,
                NomeRemetente = nomeRemetente,
                MensagemEmHtml = true
            };

            smtpUtil.Enviar();
        }
    }
}
