using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Resources;
using System;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o alterar uma viagem
    /// </summary>
    public class AlterarViagemEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// ID da viagem
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// ID do carro
        /// </summary>
        public int IdCarro { get; }

        /// <summary>
        /// ID do motorista
        /// </summary>
        public int IdMotorista { get; }

        /// <summary>
        /// ID da localidade de embarque
        /// </summary>
        public int IdLocalidadeEmbarque { get; }

        /// <summary>
        /// ID da localidade de desembarque
        /// </summary>
        public int IdLocalidadeDesembarque { get; }

        /// <summary>
        /// Descrição da viagem
        /// </summary>
        public string Descricao { get; }

        /// <summary>
        /// Valor da passagem por pessoa
        /// </summary>
        public decimal ValorPassagem { get; }

        /// <summary>
        /// Data e horário da saída
        /// </summary>
        public DateTime DataHorarioSaida { get; }

        /// <summary>
        /// Descrição dos locais de embarque
        /// </summary>
        public string[] LocaisEmbarque { get; }

        /// <summary>
        /// Descrição dos locais de desembarque
        /// </summary>
        public string[] LocaisDesembarque { get; }

        /// <summary>
        /// KM inicial
        /// </summary>
        public int? KmInicial { get; }

        /// <summary>
        /// KM final
        /// </summary>
        public int? KmFinal { get; }

        /// <summary>
        /// KM rodado
        /// </summary>
        public int? KmRodado { get; }

        /// <summary>
        /// Nome do contratante do frete
        /// </summary>
        public string NomeContratanteFrete { get; }

        /// <summary>
        /// Endereço do contratante do frete
        /// </summary>
        public string EnderecoContratanteFrete { get; }

        /// <summary>
        /// Número do contratante do frete
        /// </summary>
        public string DocumentoContratanteFrete { get; }

        /// <summary>
        /// RG do contratante do frete
        /// </summary>
        public string RgContratanteFrete { get; }

        /// <summary>
        /// Telefone do contratante do frete
        /// </summary>
        public string TelefoneContratanteFrete { get; }

        public AlterarViagemEntrada(
            int id,
            int idCarro,
            int idMotorista,
            int idLocalidadeEmbarque,
            int idLocalidadeDesembarque,
            string descricao,
            decimal valorPassagem,
            DateTime dataHorarioSaida,
            string[] locaisEmbarque,
            string[] locaisDesembarque,
            int? kmInicial = null,
            int? kmFinal = null,
            int? kmRodado = null,
            string nomeContratanteFrete = null,
            string enderecoContratanteFrete = null,
            string documentoContratanteFrete = null,
            string rgContratanteFrete = null,
            string telefoneContratanteFrete = null)
        {
            Id                        = id;
            IdCarro                   = idCarro;
            IdMotorista               = idMotorista;
            IdLocalidadeEmbarque      = idLocalidadeEmbarque;
            IdLocalidadeDesembarque   = idLocalidadeDesembarque;
            Descricao                 = descricao;
            ValorPassagem             = valorPassagem;
            DataHorarioSaida          = dataHorarioSaida;
            LocaisEmbarque            = locaisEmbarque;
            LocaisDesembarque         = locaisDesembarque;
            KmInicial                 = kmInicial;
            KmFinal                   = kmFinal;
            KmRodado                  = kmRodado;
            NomeContratanteFrete      = nomeContratanteFrete;
            EnderecoContratanteFrete  = enderecoContratanteFrete;
            DocumentoContratanteFrete = documentoContratanteFrete;
            RgContratanteFrete        = rgContratanteFrete;
            TelefoneContratanteFrete  = telefoneContratanteFrete;

            Validar();
        }

        public override string ToString()
        {
            return $"{this.Descricao} - {this.DataHorarioSaida.ToString("dd/MM/yyyy HH:mm")}";
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.Id, 0, ViagemResource.Id_Viagem_Nao_Existe)
                .NotificarSeMenorOuIgualA(this.IdCarro, 0, CarroResource.Id_Carro_Nao_Existe)
                .NotificarSeMenorOuIgualA(this.IdMotorista, 0, MotoristaResource.Id_Motorista_Nao_Existe)
                .NotificarSeMenorOuIgualA(this.IdLocalidadeEmbarque, 0, ViagemResource.Localidade_Embarque_Nao_Existe)
                .NotificarSeMenorOuIgualA(this.IdLocalidadeDesembarque, 0, ViagemResource.Localidade_Desembarque_Nao_Existe)
                .NotificarSeNuloOuVazio(this.Descricao, ViagemResource.Descricao_Obrigatoria_Nao_Informada)
                .NotificarSeMenorOuIgualA(this.ValorPassagem, 0, ViagemResource.Valor_Passagem_Obrigatorio_Nao_Informada);

            if (this.IdLocalidadeEmbarque > 0 && this.IdLocalidadeDesembarque > 0 && this.IdLocalidadeEmbarque == this.IdLocalidadeDesembarque)
                this.AdicionarNotificacao(ViagemResource.Localidade_Embarque_Desembarque_Nao_Podem_Ser_Iguais);
        }
    }
}
