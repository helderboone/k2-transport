using System;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete os parâmetros utilizados para alteração de um motorista
    /// </summary>
    public class AlterarMotoristaEntrada : AlterarUsuarioEntrada
    {
        public int Id { get; set; }

        public string Cnh { get; set; }

        public DateTime DataExpedicaoCnh { get; set; }

        public DateTime DataValidadeCnh { get; set; }

        public string Cep { get; set; }

        public string Endereco { get; set; }

        public string Municipio { get; set; }

        public string Uf { get; set; }
    }
}
