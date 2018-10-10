namespace K2.Dominio
{
    /// <summary>
    /// Situação de uma viagem
    /// </summary>
    public enum TipoSituacaoViagem
    {
        PendenteConfirmacao = 0,
        Confirmada = 1,
        Cancelada = -1
    }

    /// <summary>
    /// Classe que armazena as perfis de acesso.
    /// </summary>
    public static class TipoPerfil
    {
        public const string Administrador = "Administrador";

        public const string Motorista = "Motorista";

        public const string Cliente = "Cliente";

        public const string ProprietarioCarro = "ProprietarioCarro";
    }

    public static class ExtensionMethods
    {
        public static string ObterDescricao(this TipoSituacaoViagem tipoSituacao)
        {
            switch (tipoSituacao)
            {
                case TipoSituacaoViagem.PendenteConfirmacao:
                    return "Pendente de confirmação";
                case TipoSituacaoViagem.Confirmada:
                    return "Confirmada";
                case TipoSituacaoViagem.Cancelada:
                    return "Cancelada";
                default: return string.Empty;
            }
        }
    }
}
