
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProductManager.Api
{
    public class TokenService
    {
        public static object GenerateToken(string textoGerarToken)
        {
            var key = Encoding.ASCII.GetBytes(Key.Secret);

            var tokenConfig = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                    new Claim("Robbu", ".NetPleno")
                }),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials  = new  SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var tokenHandle = new JwtSecurityTokenHandler();
            var token = tokenHandle.CreateToken(tokenConfig);
            var tokenString = tokenHandle.WriteToken(token);

            return new
            {
                token = tokenString
            };

        }
    }
}
