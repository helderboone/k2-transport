using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace K2.Web.Helpers
{
    public class DatatablesHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DatatablesHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string PalavraChave
        {
            get { return _httpContextAccessor.HttpContext.Request.Form["search[value]"].FirstOrDefault(); }
        }

        public int Draw
        {
            get { return !_httpContextAccessor.HttpContext.Request.Form["draw"].Any()
                ? -1
                : Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["draw"].First()); }
        }

        public int PaginaIndex
        {
            get
            {
                if (string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Form["length"].FirstOrDefault())
                    || string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Form["start"].FirstOrDefault()))
                    return -1;

                var length = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["length"].FirstOrDefault());
                var start = Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["start"].FirstOrDefault());

                return (start / length) + 1;
            }
        }

        public int PaginaTamanho
        {
            get { return !_httpContextAccessor.HttpContext.Request.Form["length"].Any()
                ? -1
                : Convert.ToInt32(_httpContextAccessor.HttpContext.Request.Form["length"].FirstOrDefault()); }
        }

        public string OrdenarSentido
        {
            get { return _httpContextAccessor.HttpContext.Request.Form["order[0][dir]"].FirstOrDefault() ?? string.Empty; }
        }

        public string OrdenarPor
        {
            get
            {
                var colunaOrdenacaoIndex = -1;

                return int.TryParse(_httpContextAccessor.HttpContext.Request.Form["order[0][column]"].FirstOrDefault(), out colunaOrdenacaoIndex)
                    ? _httpContextAccessor.HttpContext.Request.Form["columns[" + colunaOrdenacaoIndex + "][data]"].FirstOrDefault()
                    : string.Empty;
            }
        }
    }
}
