using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ExternalService.Helpers.Auth
{
    public class AuthHelper : IAuthHelper
    {
        private readonly string KeyToken;
        public AuthHelper(IConfiguration configuration)
        {
            KeyToken = configuration.GetSection("Jwt:Key").Value;
        }
        public string CreateToken(IEnumerable<Claim>? claims, TimeSpan expiration)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KeyToken));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims ?? Enumerable.Empty<Claim>(),
                expires: DateTime.UtcNow.Add(expiration),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public bool ValidateToken(string token)
        {
            var key = Encoding.ASCII.GetBytes(KeyToken);

            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public byte[] CalculatePasswordHash(Guid userId, string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] userIdBytes = userId.ToByteArray();
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Concatenar los bytes del ID y la contraseña
                byte[] concatenatedData = new byte[userIdBytes.Length + passwordBytes.Length];
                Buffer.BlockCopy(userIdBytes, 0, concatenatedData, 0, userIdBytes.Length);
                Buffer.BlockCopy(passwordBytes, 0, concatenatedData, userIdBytes.Length, passwordBytes.Length);

                // Calcular el hash SHA-256
                return sha256.ComputeHash(concatenatedData);
            }
        }

    }
}
