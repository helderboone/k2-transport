using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Resources;

namespace K2.Dominio.Comandos.Entrada
{
    /// <summary>
    /// Comando utilizado para o alterar um carro
    /// </summary>
    public class AlterarCarroEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// ID do carro
        /// </summary>
        public int Id { get; }

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
        /// Capacidade disponível para passageiros
        /// </summary>
        public int Capacidade { get; }

        /// <summary>
        /// Número da placa do carro
        /// </summary>
        public string Placa { get; }

        /// <summary>
        /// Cor do carro
        /// </summary>
        public string Cor { get; }

        /// <summary>
        /// Número RENAVAM do carro
        /// </summary>
        public string Renavam { get; }

        /// <summary>
        /// Número de registro Seturb do carro
        /// </summary>
        public string RegistroSeturb { get; }

        /// <summary>
        /// Descrição das características do carro
        /// </summary>
        public string[] Caracteristicas { get; }

        public AlterarCarroEntrada(
            int id,
            int idProprietario,
            string descricao,
            string nomeFabricante,
            string anoModelo,
            int capacidade,
            string placa,
            string renavam,
            string[] caracteristicas,
            string cor,
            string registroSeturb)
        {
            Id                   = id;
            IdProprietario       = idProprietario;
            Descricao            = descricao?.ToUpper();
            NomeFabricante       = nomeFabricante?.ToUpper();
            AnoModelo            = anoModelo;
            Capacidade           = capacidade;
            Placa                = placa?.ToUpper();
            Renavam              = renavam;
            Caracteristicas      = caracteristicas;
            Cor                  = cor?.ToUpper();
            RegistroSeturb       = registroSeturb?.ToUpper();

            this.Validar();
        }

        public override string ToString()
        {
            return this.Descricao;
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdProprietario, 0, CarroResource.Id_Proprietario_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.Descricao, CarroResource.Descricao_Obrigatoria_Nao_Informada)
                .NotificarSeNuloOuVazio(this.Placa, CarroResource.Placa_Obrigatoria_Nao_Informado)
                .NotificarSeMenorOuIgualA(this.Capacidade, 0, CarroResource.Capacidade_Invalida)
                .NotificarSeNuloOuVazio(this.Cor, CarroResource.Cor_Obrigatoria_Nao_Informada)
                .NotificarSeNuloOuVazio(this.RegistroSeturb, CarroResource.NumeroRegistroSeturb_Obrigatorio_Nao_Informado);
        }
    }
}
