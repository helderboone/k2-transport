using Microsoft.AspNetCore.Html;
using System.Text;

namespace K2.Web.Helpers
{
    public static class HtmlHelpers
    {
        public static HtmlString DropDownUf(string id, string cssClass, string valor, string atributosHtml = "style=\"width: 100%;\"")
        {
            var atribId = !string.IsNullOrEmpty(id) ? " id=\"" + id + "\" name=\"" + id + "\"" : string.Empty;

            var html = new StringBuilder($"<select{atribId} class=\"{cssClass}\"");

            if (!string.IsNullOrEmpty(atributosHtml))
                html.Append(atributosHtml);

            html.AppendLine(">");

            html.AppendLine("<option value=\"\"></option>");
            html.AppendLine("<option value=\"AC\"" + (valor == "AC" ? " selected" : string.Empty) + ">Acre</option>");
            html.AppendLine("<option value=\"AL\"" + (valor == "AL" ? " selected" : string.Empty) + ">Alagoas</option>");
            html.AppendLine("<option value=\"AP\"" + (valor == "AP" ? " selected" : string.Empty) + ">Amapá</option>");
            html.AppendLine("<option value=\"AM\"" + (valor == "AM" ? " selected" : string.Empty) + ">Amazonas</option>");
            html.AppendLine("<option value=\"BA\"" + (valor == "BA" ? " selected" : string.Empty) + ">Bahia</option>");
            html.AppendLine("<option value=\"CE\"" + (valor == "CE" ? " selected" : string.Empty) + ">Ceará</option>");
            html.AppendLine("<option value=\"DF\"" + (valor == "DF" ? " selected" : string.Empty) + ">Distrito Federal</option>");
            html.AppendLine("<option value=\"ES\"" + (valor == "ES" ? " selected" : string.Empty) + ">Espírito Santo</option>");
            html.AppendLine("<option value=\"GO\"" + (valor == "GO" ? " selected" : string.Empty) + ">Goiás</option>");
            html.AppendLine("<option value=\"MA\"" + (valor == "MA" ? " selected" : string.Empty) + ">Maranhão</option>");
            html.AppendLine("<option value=\"MT\"" + (valor == "MT" ? " selected" : string.Empty) + ">Mato Grosso</option>");
            html.AppendLine("<option value=\"MS\"" + (valor == "MS" ? " selected" : string.Empty) + ">Mato Grosso do Sul</option>");
            html.AppendLine("<option value=\"MG\"" + (valor == "MG" ? " selected" : string.Empty) + ">Minas Gerais</option>");
            html.AppendLine("<option value=\"PA\"" + (valor == "PA" ? " selected" : string.Empty) + ">Pará</option>");
            html.AppendLine("<option value=\"PB\"" + (valor == "PB" ? " selected" : string.Empty) + ">Paraíba</option>");
            html.AppendLine("<option value=\"PR\"" + (valor == "PR" ? " selected" : string.Empty) + ">Paraná</option>");
            html.AppendLine("<option value=\"PE\"" + (valor == "PE" ? " selected" : string.Empty) + ">Pernambuco</option>");
            html.AppendLine("<option value=\"PI\"" + (valor == "PI" ? " selected" : string.Empty) + ">Piauí</option>");
            html.AppendLine("<option value=\"RJ\"" + (valor == "RJ" ? " selected" : string.Empty) + ">Rio de Janeiro</option>");
            html.AppendLine("<option value=\"RN\"" + (valor == "RN" ? " selected" : string.Empty) + ">Rio Grande do Norte</option>");
            html.AppendLine("<option value=\"RS\"" + (valor == "RS" ? " selected" : string.Empty) + ">Rio Grande do Sul</option>");
            html.AppendLine("<option value=\"RO\"" + (valor == "RO" ? " selected" : string.Empty) + ">Rondônia</option>");
            html.AppendLine("<option value=\"RR\"" + (valor == "RR" ? " selected" : string.Empty) + ">Roraima</option>");
            html.AppendLine("<option value=\"SC\"" + (valor == "SC" ? " selected" : string.Empty) + ">Santa Catarina</option>");
            html.AppendLine("<option value=\"SP\"" + (valor == "SP" ? " selected" : string.Empty) + ">São Paulo</option>");
            html.AppendLine("<option value=\"SE\"" + (valor == "SE" ? " selected" : string.Empty) + ">Sergipe</option>");
            html.AppendLine("<option value=\"TO\"" + (valor == "TO" ? " selected" : string.Empty) + ">Tocantins</option>");

            html.Append("</select>");

            return new HtmlString(html.ToString());
        }
    }
}
