using JNogueira.Infraestrutura.NotifiqueMe;
using K2.Dominio.Comandos.Entrada;
using K2.Dominio.Comandos.Saida;
using K2.Dominio.Interfaces.Comandos;
using K2.Dominio.Interfaces.Infraestrutura.Dados.Repositorios;
using K2.Dominio.Interfaces.Servicos;
using System.Threading.Tasks;

namespace K2.Dominio.Servicos
{
    public class LogServico : Notificavel, ILogServico
    {
        private readonly ILogRepositorio _logRepositorio;

        public LogServico(ILogRepositorio logRepositorio)
        {
            _logRepositorio = logRepositorio;
        }

        public async Task<ISaida> ObterRegistroPorId(int id)
        {
            var registro = await _logRepositorio.ObterPorId(id);

            return new Saida(true, null, new LogSaida(registro));
        }

        public async Task<ISaida> ProcurarRegistros(ProcurarLogEntrada entrada)
        {
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            // Verifica se os parâmetros para a procura foram informadas corretamente
            return entrada.Invalido
                ? new Saida(false, entrada.Mensagens, null)
                : await _logRepositorio.Procurar(entrada);
        }
    }
}
