using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ChatAppServer.Utils
{
    public class JwtHelper
    {
        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int ExpiryMinutes { get; set; }

        public static TokenValidationParameters? ValidationParameters { get; set; }
        public void SetValidationParameters() {
            ValidationParameters = new TokenValidationParameters() {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key!)),
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidAudience= Audience,
                ValidateIssuer = true,
                ValidIssuer = Issuer,
                ValidateIssuerSigningKey = true
            };
        }
        public string GenerateToken(string username)
        {
            
            var tokenHandler = new JsonWebTokenHandler();
            var key = Encoding.ASCII.GetBytes(Key!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, username), // Standard 'sub' claim for user identity
                    new Claim(ClaimTypes.Name, username), // Custom claim
                    new Claim(ClaimTypes.Role, "LoggedInUser")
                }),

                Expires = DateTime.UtcNow.AddMinutes(ExpiryMinutes),
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }

        public static async Task<bool> ValidateToken(string token)
        {
            // Validate the JWT token
            var tokenHandler = new JsonWebTokenHandler();
            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, ValidationParameters);
            
            return tokenValidationResult.IsValid;
            
        }
    }
}
