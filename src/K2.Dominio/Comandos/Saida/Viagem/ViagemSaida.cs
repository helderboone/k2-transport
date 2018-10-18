﻿using K2.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace K2.Dominio.Comandos.Saida
{
    /// <summary>
    /// Classe que representa uma viagem
    /// </summary>
    public class ViagemSaida
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
        /// Situação da viagem
        /// </summary>
        public int Situacao { get; }

        /// <summary>
        /// Descrição da situação da viagem
        /// </summary>
        public string DescricaoSituacao { get; }

        /// <summary>
        /// Descrição dos locais de embarque
        /// </summary>
        public string[] LocaisEmbarque { get; }

        /// <summary>
        /// Descrição dos locais de desembarque
        /// </summary>
        public string[] LocaisDesembarque { get; }

        /// <summary>
        /// Descrição do motivo de cancelamento da viagem
        /// </summary>
        public string DescricaoCancelamento { get; }

        /// <summary>
        /// Carro relacionado a viagem
        /// </summary>
        public object Carro { get; }

        /// <summary>
        /// Motorista relacionado a viagem
        /// </summary>
        public object Motorista { get; }

        /// <summary>
        /// Localidade de embarque relacionado a viagem
        /// </summary>
        public object LocalidadeEmbarque { get; }

        /// <summary>
        /// Localidade de desembarque relacionado a viagem
        /// </summary>
        public object LocalidadeDesembarque { get; }

        /// <summary>
        /// Reservas da viagem
        /// </summary>
        public IEnumerable<object> Reservas { get; }

        /// <summary>
        /// Percentual de ocupação da viagem
        /// </summary>
        public string PercentualDisponibilidade { get; }

        /// <summary>
        /// Quantidade de lugares disponíveis
        /// </summary>
        public int QuantidadeLugaresDisponiveis { get; }

        public ViagemSaida(Viagem viagem)
        {
            Id                      = viagem.Id;
            IdCarro                 = viagem.IdCarro;
            IdMotorista             = viagem.IdMotorista;
            IdLocalidadeEmbarque    = viagem.IdLocalidadeEmbarque;
            IdLocalidadeDesembarque = viagem.IdLocalidadeDesembarque;
            Descricao               = viagem.Descricao;
            ValorPassagem           = viagem.ValorPassagem;
            DataHorarioSaida        = viagem.DataHorarioSaida;
            Situacao                = viagem.Situacao;
            DescricaoSituacao       = viagem.ObterTipoSituacao().ObterDescricao();
            LocaisEmbarque          = viagem.Embarques?.Split(";".ToCharArray());
            LocaisDesembarque       = viagem.Desembarques?.Split(";".ToCharArray());
            DescricaoCancelamento   = viagem.DescricaoCancelamento;
            Carro = new
            {
                viagem.Carro.Descricao,
                NomeProprietario = viagem.Carro.Proprietario.Nome,
                viagem.Carro.AnoModelo,
                viagem.Carro.NomeFabricante,
                viagem.Carro.Renavam,
                Caracteristicas = viagem.Carro.Caracteristicas.Split(";".ToCharArray()),
                viagem.Carro.Placa,
                viagem.Carro.QuantidadeLugares
            };
            Motorista = new
            {
                viagem.Motorista.Nome,
                viagem.Motorista.Cnh,
                viagem.Motorista.Cpf,
                viagem.Motorista.Celular
            };
            LocalidadeEmbarque = new
            {
                viagem.LocalidadeEmbarque.Nome,
                viagem.LocalidadeEmbarque.Uf
            };
            LocalidadeDesembarque = new
            {
                viagem.LocalidadeDesembarque.Nome,
                viagem.LocalidadeDesembarque.Uf
            };
            Reservas = viagem.Reservas.Select(x => new
            {
                x.Id,
                x.IdCliente,
                Cliente = new {
                    x.Cliente.Nome,
                    x.Cliente.Cpf,
                    x.Cliente.Rg,
                    x.Cliente.Celular
                },
                x.Observacao,
                x.ValorPago,
                Dependente = x.Dependente != null ? new
                {
                    x.Dependente.Nome,
                    x.Dependente.DataNascimento,
                    x.Dependente.Cpf,
                    x.Dependente.Rg
                } : null
            }).ToArray();
            QuantidadeLugaresDisponiveis = viagem.QuantidadeLugaresDisponiveis;
            PercentualDisponibilidade    = viagem.PercentualDisponibilidade.ToString("N0") + "%";
        }

        public override string ToString()
        {
            return $"{this.Descricao} - {this.DataHorarioSaida.ToString("dd/MM/yyyy HH:mm")}";
        }
    }
}
