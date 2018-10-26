namespace K2.Dominio
{
    /// <summary>
    /// Situação de uma viagem
    /// </summary>
    public enum TipoSituacaoViagem
    {
        PendenteConfirmacao = 0,
        Confirmada = 1,
        Realizada = 2,
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

    /// <summary>
    /// Tipos de políticas de acesso utilizadas
    /// </summary>
    public static class TipoPoliticaAcesso
    {
        /// <summary>
        /// Somente usuários com o perfil "administrador" terão acesso
        /// </summary>
        public const string Administrador = "Administrador";

        /// <summary>
        /// Somente usuários com o perfil "administrador" ou "motorista" terão acesso
        /// </summary>
        public const string Motorista = "Motorista";

        /// <summary>
        /// Somente usuários com o perfil "administrador" ou "proprietário" terão acesso
        /// </summary>
        public const string ProprietarioCarro = "ProprietarioCarro";

        /// <summary>
        /// Somente usuários com o perfil "administrador", "motorista" ou "proprietário" terão acesso
        /// </summary>
        public const string MotoristaOuProprietarioCarro = "MotoristaOuProprietarioCarro";

        /// <summary>
        /// Somente usuário com o perfil "administrador" e o e-mail "jlnpinheiro@gmail.com"
        /// </summary>
        public const string AnalistaTI = "MotoristaOuProprietarioCarro";
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
