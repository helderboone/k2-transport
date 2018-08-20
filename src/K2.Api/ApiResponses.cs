using K2.Dominio.Comandos.Saida;
using System;

namespace K2.Api
{
    /// <summary>
    /// Response padrão da API para o erro HTTP 401
    /// </summary>
    public class UnauthorizedApiResponse : Saida
    {
        public UnauthorizedApiResponse()
            : base(false, new[] { "Erro 401: Acesso negado. Certifique-se que você foi autenticado." }, null)
        {

        }
    }

    /// <summary>
    /// Response padrão da API para o erro HTTP 403
    /// </summary>
    public class ForbiddenApiResponse : Saida
    {
        public ForbiddenApiResponse()
            : base (false, new[] { "Erro 403: Acesso negado. Você não tem permissão de acesso para essa funcionalidade." }, null)
        {

        }
    }

    /// <summary>
    /// Response padrão da API para o erro HTTP 404
    /// </summary>
    public class NotFoundApiResponse : Saida
    {
        public NotFoundApiResponse()
            : base(false, new[] { "Erro 404: O endereço não encontrado." }, null)
        {

        }

        public NotFoundApiResponse(string path)
            : base(false, new[] { $"Erro 404: O endereço \"{path}\" não foi encontrado." }, null)
        {

        }
    }

    /// <summary>
    /// Response padrão da API para o erro HTTP 415
    /// </summary>
    public class UnsupportedMediaTypeApiResponse : Saida
    {
        public UnsupportedMediaTypeApiResponse(string requestContentType)
            : base(false, new[] { $"Erro 415: O tipo de requisição \"{requestContentType}\" não é suportado pela API." }, null)
        {

        }
    }

    /// <summary>
    /// Response padrão da API para o erro HTTP 500
    /// </summary>
    public class InternalServerErrorApiResponse : Saida
    {
        public InternalServerErrorApiResponse(Exception exception)
            : base (false, new[] { exception.Message }, new
            {
                Exception = exception.Message,
                BaseException = exception.GetBaseException().Message,
                exception.Source
            })
        {

        }
    }
}
