using GStatsFaker.DBContexts;
using GStatsFaker.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GStatsFaker.Model;

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
            User? u1 = GSFContext.Users.SingleOrDefault((u) => u.Email == Email);
            if (u1 == null) return null;
            User u = u1 ?? default!;
            bool PasswordCorrect = SecurePasswordHasher.Verify(password, u.Password);
            if (GSFContext.Users.Include(u => u.EmalVerifikations).Any((u => u.Email == Email&&u.EmalVerifikations.Any(e => e.IsVerifiziert))))
            {
                if (PasswordCorrect)
                {
                    return GenToken(Email);
                }
            }
            else
            {
                return "lol";
            }
            return null;
        }

        public string GenToken(string Email)
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
    }
}
