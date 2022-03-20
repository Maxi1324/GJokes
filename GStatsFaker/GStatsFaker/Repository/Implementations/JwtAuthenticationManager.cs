using GStatsFaker.DBContexts;
using GStatsFaker.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GStatsFaker.Repository.Implementations
{
    public class JwtAuthenticationManager : IJwtAuthenticationManager
    {
        public GSFContext GSFContext { get; set; }

        public JwtAuthenticationManager(GSFContext Context)
        {
            GSFContext = Context;
        }

        public string? Authenticate(string Email, string password)
        {
            if (GSFContext.Users.Any((u) => u.Email == Email && u.Password == password) &&
                GSFContext.Users.Any((u=>u.EmalVerifikations.Any(e => e.IsVerifiziert)))) 
            { 
                
                JwtSecurityTokenHandler Handler = new JwtSecurityTokenHandler();
              
                byte[] tokenKey = Encoding.ASCII.GetBytes(Config.key);
                SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[] {
                        new Claim(ClaimTypes.Email, Email)
                    }),
                    Expires = DateTime.UtcNow.AddHours(Config.JWTExpireTime),
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature
                        )
                };
                
                SecurityToken token = Handler.CreateToken(tokenDescriptor);
                return Handler.WriteToken(token);
            }
            else
            {
                return null;
            }
        }
    }
}
