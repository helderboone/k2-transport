using JNogueira.Infraestrutura.Utilzao;
using K2.Infraestrutura;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace K2.Web.Models
{
    /// <summary>
    /// Classe que reflete as informações de saída de uma viagem
    /// </summary>
    public class ViagemSaida : Saida<ViagemRegistro>
    {
        public ViagemSaida(bool sucesso, IEnumerable<string> mensagens, ViagemRegistro retorno)
            : base(sucesso, mensagens, retorno)
        {
            
        }

        public static ViagemSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ViagemSaida>(json)
                : throw new Exception("A saida da API foi nula ou vazia.");
        }
    }

    /// <summary>
    /// Classe que reflete as informações de saída de uma viagem
    /// </summary>
    public class ViagensSaida : Saida<IEnumerable<ViagemRegistro>>
    {
        public ViagensSaida(bool sucesso, IEnumerable<string> mensagens, IEnumerable<ViagemRegistro> retorno)
            : base(sucesso, mensagens, retorno)
        {

        }

        public static ViagensSaida Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<ViagensSaida>(json)
                : throw new Exception("A saida da API foi nula ou vazia.");
        }
    }

    public class ViagemRegistro
    {
        public int Id { get; set; }

        public int IdCarro { get; set; }

        public int IdMotorista { get; set; }

        public int IdLocalidadeEmbarque { get; set; }

        public int IdLocalidadeDesembarque { get; set; }

        public string Descricao { get; set; }

        public decimal ValorPassagem { get; set; }

        public DateTime DataHorarioSaida { get; set; }

        public int Situacao { get; set; }

        public string DescricaoSituacao { get; set; }

        public string[] LocaisEmbarque { get; set; }

        public string[] LocaisDesembarque { get; set; }

        public int? KmInicial { get; set; }

        public int? KmFinal { get; set; }

        public int? KmRodado { get; set; }

        public string NomeContratanteFrete { get; set; }

        public string EnderecoContratanteFrete { get; set; }

        public string DocumentoContratanteFrete { get; set; }

        public string DocumentoContratanteFreteFormatado => this.DocumentoContratanteFrete?.FormatarCpfCnpj();

        public string RgContratanteFrete { get; set; }

        public string TelefoneContratanteFrete { get; set; }

        public string TelefoneContratanteFreteFormatado => this.TelefoneContratanteFrete?.Formatar("(##)######-####");

        public ViagemCarroRetorno Carro { get; set; }

        public ViagemMotoristaRetorno Motorista { get; set; }

        public ViagemLocalidadeRetorno LocalidadeEmbarque { get; set; }

        public ViagemLocalidadeRetorno LocalidadeDesembarque { get; set; }

        public IEnumerable<ViagemReservaRetorno> Reservas { get; set; }

        public string PercentualDisponibilidade { get; set; }

        public int QuantidadeLugaresDisponiveis { get; set; }

        public string DataHorarioSaidaToString => this.DataHorarioSaida.ToString("dd MMM, yyyy - HH:mm");

        public string DiaSemanaSaida => this.DataHorarioSaida.ToString("dddd");

        public double QuantidadeDiasSaida => Math.Round(this.DataHorarioSaida.Subtract(DateTimeHelper.ObterHorarioAtualBrasilia()).TotalDays, 0);

        public decimal ValorArrecadadoReservas => this.Reservas.Where(x => x.ValorPago.HasValue).Sum(x => x.ValorPago.Value);

        public decimal PercentualArrecadadoReservas => Math.Round(100 * this.ValorArrecadadoReservas / (this.Carro.Capacidade * this.ValorPassagem), 0);
    }

    public class ViagemCarroRetorno
    {
        public string Descricao { get; set; }

        public string NomeProprietario { get; set; }

        public string AnoModelo { get; set; }

        public string NomeFabricante { get; set; }

        public string Renavam { get; set; }

        public string[] Caracteristicas { get; set; }

        public string Placa { get; set; }

        public int Capacidade { get; set; }

        public string Cor { get; set; }

        public string RegistroSeturb { get; set; }
    }

    public class ViagemMotoristaRetorno
    {
        public string Nome { get; set; }

        public string Cnh { get; set; }

        public string Cpf { get; set; }

        public string CpfFormatado => this.Cpf?.FormatarCpf();

        public string Celular { get; set; }

        public string CelularFormatado => this.Celular?.Formatar("(##)######-####");

        public DateTime DataExpedicaoCnh { get; set; }

        public string DataExpedicaoCnhToString => this.DataExpedicaoCnh.ToString("dd/MM/yyyy");

        public DateTime DataValidadeCnh { get; set; }

        public string DataValidadeCnhToString => this.DataValidadeCnh.ToString("dd/MM/yyyy");

        public string Cep { get; set; }

        public string Endereco { get; set; }

        public string Municipio { get; set; }

        public string Uf { get; set; }

        public string EnderecoCompleto
        {
            get
            {
                var enderecoCompleto = new List<string>();

                if (!string.IsNullOrEmpty(this.Endereco))
                    enderecoCompleto.Add(this.Endereco);

                if (!string.IsNullOrEmpty(this.Municipio))
                    enderecoCompleto.Add(this.Municipio);

                if (!string.IsNullOrEmpty(this.Uf))
                    enderecoCompleto.Add(this.Uf);

                if (!string.IsNullOrEmpty(this.Cep))
                    enderecoCompleto.Add("CEP: " + this.Cep);

                return string.Join(", ", enderecoCompleto);
            }
        }
    }

    public class ViagemLocalidadeRetorno
    {
        public string Nome  { get; set; }

        public string Sigla { get; set; }

        public string Uf { get; set; }
    }

    public class ViagemReservaRetorno
    {
        public int Id { get; set; }

        public int IdCliente { get; set; }

        public string Observacao { get; set; }

        public decimal? ValorPago { get; set; }

        public string LocalEmbarque { get; set; }

        public string LocalDesembarque { get; set; }

        public int SequenciaEmbarque { get; set; }

        public string ValorPagoFormatado => this.ValorPago?.ToString("C2");

        public ViagemReservaClienteRetorno Cliente { get; set; }

        public ViagemReservaDependenteRetorno Dependente { get; set; }
    }

    public class ViagemReservaClienteRetorno
    {
        public string Nome { get; set; }

        public string Cpf { get; set; }

        public string CpfFormatado => this.Cpf?.FormatarCpf();

        public string Rg { get; set; }

        public string Celular { get; set; }

        public string CelularFormatado => this.Celular?.Formatar("(##)######-####");
    }

    public class ViagemReservaDependenteRetorno
    {
        public string Nome { get; set; }

        public DateTime DataNascimento { get; set; }

        public string DataNascimentoToString => this.DataNascimento.ToString("dd/MM/yyyy");

        public string Cpf { get; set; }

        public string CpfFormatado => this.Cpf?.FormatarCpf();

        public string Rg { get; set; }
    }
}
