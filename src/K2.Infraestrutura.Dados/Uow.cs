using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Interfaces.Dados;
using K2.Dominio.Resources;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace K2.Infraestrutura.Dados
{
    /// <summary>
    /// Implementação do padrão Unit Of Work
    /// </summary>
    public class Uow : Notificavel, IUow
    {
        private readonly EfDataContext _context;

        public Uow(EfDataContext context)
        {
            _context = context;
        }

        public async Task Commit()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                var mensagemException = dbEx.GetBaseException().Message;

                if (mensagemException.Contains("add or update")) // insert ou update
                {
                    this.NotificarSeVerdadeiro(mensagemException.Contains("fk_reserva_cliente"), ClienteResource.Nao_Possivel_Excluir_Cliente_Com_Reservas);
                }
                else if (mensagemException.Contains("delete")) // delete
                {
                    this.NotificarSeVerdadeiro(mensagemException.Contains("fk_reserva_cliente"), ClienteResource.Nao_Possivel_Excluir_Cliente_Com_Reservas);
                    this.NotificarSeVerdadeiro(mensagemException.Contains("fk_reserva_viagem"), ViagemResource.Nao_Possivel_Excluir_Viagem_Com_Reservas);

                    this.NotificarSeVerdadeiro(mensagemException.Contains("fk_viagem_localidade_embarque") || mensagemException.Contains("fk_viagem_localidade_desembarque"), LocalidadeResource.Nao_Possivel_Excluir_Localidade_Com_Viagens);
                    this.NotificarSeVerdadeiro(mensagemException.Contains("fk_viagem_carro"), CarroResource.Nao_Possivel_Excluir_Carro_Com_Viagens);
                    this.NotificarSeVerdadeiro(mensagemException.Contains("fk_viagem_motorista"), MotoristaResource.Nao_Possivel_Excluir_Motorista_Com_Viagens);

                    this.NotificarSeVerdadeiro(mensagemException.Contains("fk_carro_proprietario"), ProprietarioCarroResource.Nao_Possivel_Excluir_Proprietario_Com_Carros);
                }
                else
                {
                    this.AdicionarNotificacao($"Não é possível salvar as alterações no banco de dados: {mensagemException}");
                }
            }
        }
    }
}
