using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Interfaces.Dados;
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

                //if (mensagemException.Contains("add or update")) // insert ou update
                //{
                //    this.NotificarSeVerdadeiro(mensagemException.Contains("REFERENCES `categoria`"), CategoriaMensagem.Id_Categoria_Nao_Existe);

                //}
                if (mensagemException.Contains("delete")) // delete
                {
                    //this.NotificarSeVerdadeiro(mensagemException.Contains("REFERENCES `conta`"), ContaMensagem.Conta_Excluir_Erro_FK);
                    //this.NotificarSeVerdadeiro(mensagemException.Contains("REFERENCES `cartaocredito`"), CartaoCreditoMensagem.Cartao_Excluir_Erro_FK);
                    //this.NotificarSeVerdadeiro(mensagemException.Contains("REFERENCES `categoria`"), CategoriaMensagem.Categoria_Excluir_Erro_FK);
                }
                else
                {
                    this.AdicionarNotificacao($"Não é possível salvar as alterações no banco de dados: {mensagemException}");
                }
            }
        }
    }
}
