using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Resources;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o cadastro de um carro
    /// </summary>
    public class CadastrarCarroEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// ID do proprietário
        /// </summary>
        public int IdProprietario { get; }

        /// <summary>
        /// Descrição do carro
        /// </summary>
        public string Descricao { get; }

        /// <summary>
        /// Nome do fabricante do carro
        /// </summary>
        public string NomeFabricante { get; }

        /// <summary>
        /// Ano/modelo de fabricação do carro
        /// </summary>
        public string AnoModelo { get; }

        /// <summary>
        /// Quantidade de lugares disponíveis para passageiros
        /// </summary>
        public int QuantidadeLugares { get; }

        /// <summary>
        /// Número da placa do carro
        /// </summary>
        public string Placa { get; }

        /// <summary>
        /// Número RENAVAM do carro
        /// </summary>
        public string Renavam { get; }

        /// <summary>
        /// Descrição das características do carro
        /// </summary>
        public string[] Caracteristicas { get; }

        public CadastrarCarroEntrada(
            int idProprietario,
            string descricao,
            string nomeFabricante,
            string anoModelo,
            int quantidadeLugares,
            string placa,
            string renavam,
            string[] caracteristicas)
        {
            IdProprietario    = idProprietario;
            Descricao         = descricao?.ToUpper();
            NomeFabricante    = nomeFabricante?.ToUpper();
            AnoModelo         = anoModelo;
            QuantidadeLugares = quantidadeLugares;
            Placa             = placa?.ToUpper();
            Renavam           = renavam;
            Caracteristicas   = caracteristicas;

            this.Validar();
        }

        public override string ToString()
        {
            return this.Descricao.ToUpper();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdProprietario, 0, CarroResource.Id_Proprietario_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.Descricao, CarroResource.Descricao_Obrigatoria_Nao_Informada)
                .NotificarSeNuloOuVazio(this.NomeFabricante, CarroResource.Nome_Fabricante_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.AnoModelo, CarroResource.Ano_Modelo_Obrigatorio_Nao_Informado)
                .NotificarSeMenorOuIgualA(this.QuantidadeLugares, 0, CarroResource.Quantidade_Lugares_Invalida);
        }
    }
}
