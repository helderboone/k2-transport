using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace K2.Web
{
    public class CookieHelper
    {
        private readonly HttpContext _context;

        public CookieHelper(HttpContext context)
        {
            _context = context;
        }

        public int ObterIdUsuario() => Convert.ToInt32(_context.User.Claims.First(x => x.Type == "IdUsuario").Value);

        public string ObterNomeUsuario() => Convert.ToString(_context.User.Claims.First(x => x.Type == "Nome").Value);

        public string ObterPrimeiroNomeUsuario() => ObterNomeUsuario().Split(" ".ToCharArray())[0];

        public string ObterEmailUsuario() => Convert.ToString(_context.User.Claims.ElementAt(1).Value);

        public string ObterPerfilUsuario() => Convert.ToString(_context.User.Claims.First(x => x.Type == "Perfil").Value);
    }
}
