using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Hotel.WebApi.JWTProvider
{
    public class JwtSecurityKey
    {
        public static SymmetricSecurityKey Create(string secret)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret));
        }
    }
}