namespace K2.Web
{
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
    }
    
    /// <summary>
    /// Classe que armazena as perfis de acesso.
    /// </summary>
    public static class TipoPerfil
    {
        public const string Administrador = "Administrador";

        public const string Motorista = "Motorista";

        public const string ProprietarioCarro = "ProprietarioCarro";
    }
}
