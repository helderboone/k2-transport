using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Entidades;
using System.Threading.Tasks;

namespace K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios
{
    public interface IUsuarioRepositorio
    {
        /// <summary>
        /// Obtém um usuário a partir do seu ID
        /// </summary>
        Task<Usuario> ObterPorId(int id, bool habilitarTracking = false);

        /// <summary>
        /// Obtém um usuário a partir do seu email
        /// </summary>
        Task<Usuario> ObterPorEmail(string email, bool habilitarTracking = false);

        /// <summary>
        /// Obtém um usuário a partir do seu e-mail e senha
        /// </summary>
        Task<Usuario> ObterPorEmailSenha(string email, string senha, bool habilitarTracking = false);

        /// <summary>
        /// Obtém os usuários baseados nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarUsuarioEntrada entrada);

        /// <summary>
        /// Verifica se existe um usuário com o mesmo e-mail
        /// </summary>
        Task<bool> VerificarExistenciaPorEmail(string email, int? idUsuario = null);

        /// <summary>
        /// Verifica se existe um usuário com o mesmo CPF
        /// </summary>
        Task<bool> VerificarExistenciaPorCpf(string cpf, int? idUsuario = null);

        /// <summary>
        /// Verifica se existe um usuário com o mesmo RG
        /// </summary>
        Task<bool> VerificarExistenciaPorRg(string rg, int? idUsuario = null);

        /// <summary>
        /// Insere uma novo usuário
        /// </summary>
        Task Inserir(Usuario usuario);

        /// <summary>
        /// Deleta um usuário
        /// </summary>
        void Deletar(Usuario usuario);
    }
}
