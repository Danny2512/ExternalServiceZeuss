using System.Security.Claims;

namespace ExternalService.Helpers.Auth
{
    public interface IAuthHelper
    {
        byte[] CalculatePasswordHash(Guid userId, string password);
        string CreateToken(IEnumerable<Claim>? claims, TimeSpan expiration);
        bool ValidateToken(string token);
    }
}
