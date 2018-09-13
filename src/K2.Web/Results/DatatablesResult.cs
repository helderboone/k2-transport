﻿using K2.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace K2.Web
{
    public class DatatablesResult : IActionResult
    {
        private readonly int _draw;
        private readonly ICollection _itens;
        private readonly int _totalRegistros;

        public DatatablesResult(int draw, ProcurarSaida saida)
        {
            _draw = draw;
            _itens = saida.ObterRetorno().Registros.ToList();
            _totalRegistros = saida.ObterRetorno().TotalRegistros;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
            var jsonResult = new JsonResult(new
            {
                _draw,
                recordsTotal    = _totalRegistros,
                recordsFiltered = _totalRegistros,
                data            = _itens
            });

            await jsonResult.ExecuteResultAsync(context);
        }
    }
}