using K2.Dominio.Comandos.Entrada;
using System.Linq;

namespace K2.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um carro
    /// </summary>
    public class Carro
    {
        /// <summary>
        /// ID do carro
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// ID do proprietário
        /// </summary>
        public int IdProprietario { get; private set; }

        /// <summary>
        /// Descrição do carro
        /// </summary>
        public string Descricao { get; private set; }

        /// <summary>
        /// Nome do fabricante do carro
        /// </summary>
        public string NomeFabricante { get; private set; }

        /// <summary>
        /// Ano/modelo de fabricação do carro
        /// </summary>
        public string AnoModelo { get; private set; }

        /// <summary>
        /// Quantidade de lugares disponíveis para passageiros
        /// </summary>
        public int QuantidadeLugares { get; private set; }

        /// <summary>
        /// Número da placa do carro
        /// </summary>
        public string Placa { get; private set; }

        /// <summary>
        /// Número RENAVAM do carro
        /// </summary>
        public string Renavam { get; private set; }

        /// <summary>
        /// Descrição das características do carro
        /// </summary>
        public string Caracteristicas { get; private set; }

        /// <summary>
        /// Proprietário do carro
        /// </summary>
        public ProprietarioCarro Proprietario { get; private set; }

        private Carro()
        {

        }

        public Carro(CadastrarCarroEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.IdProprietario    = entrada.IdProprietario;
            this.Descricao         = entrada.Descricao;
            this.NomeFabricante    = entrada.NomeFabricante;
            this.AnoModelo         = entrada.AnoModelo;
            this.QuantidadeLugares = entrada.QuantidadeLugares;
            this.Placa             = entrada.Placa;
            this.Renavam           = entrada.Renavam;
            this.Caracteristicas   = entrada.Caracteristicas != null && entrada.Caracteristicas.Any()
                ? string.Join(";", entrada.Caracteristicas)
                : null;
        }

        public void Alterar(AlterarCarroEntrada entrada)
        {
            if (entrada.Invalido || entrada.Id != this.Id)
                return;

            this.IdProprietario = entrada.IdProprietario;
            this.Descricao = entrada.Descricao;
            this.NomeFabricante = entrada.NomeFabricante;
            this.AnoModelo = entrada.AnoModelo;
            this.QuantidadeLugares = entrada.QuantidadeLugares;
            this.Placa = entrada.Placa;
            this.Renavam = entrada.Renavam;
            this.Caracteristicas = entrada.Caracteristicas != null && entrada.Caracteristicas.Any()
                ? string.Join(";", entrada.Caracteristicas)
                : null;
        }

        public override string ToString()
        {
            return this.Descricao.ToUpper();
        }
    }
}
