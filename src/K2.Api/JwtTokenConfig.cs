using Microsoft.IdentityModel.Tokens;

namespace K2.Api
{
    /// <summary>
    /// Classe que armazena as configurações do token JWT
    /// </summary>
    public class JwtTokenConfig
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int ExpiracaoEmDias { get; set; }

        // A propriedade Key, à qual será vinculada uma instância da classe SecurityKey (namespace Microsoft.IdentityModel.Tokens) 
        // armazenando a chave de criptografia utilizada na criação de tokens;
        public SecurityKey Key { get; }

        // A propriedade SigningCredentials, que receberá um objeto baseado em uma classe também chamada SigningCredentials (namespace Microsoft.IdentityModel.Tokens). 
        // Esta referência conterá a chave de criptografia e o algoritmo de segurança empregados na geração de assinaturas digitais para tokens
        public SigningCredentials SigningCredentials { get; }

        public JwtTokenConfig()
        {
            Key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes("B4C7EC2C-D6F9-43D7-9DF5-267BDD1C73DB"));

            SigningCredentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
    }
}
