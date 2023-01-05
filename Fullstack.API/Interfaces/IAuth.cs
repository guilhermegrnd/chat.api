using Fullstack.API.Models;
using System.Security.Claims;

namespace Fullstack.API.Interfaces
{
    public interface IAuth
    {
        public Task<Auth> Authenticate(User user, bool alreadyHashed = false);
        public Task<Auth> RefreshTokenAsync(Auth request);
        public Task<bool> DeleteRefreshToken(ClaimsIdentity identity);
        public Task<long> GetAuthenticatedUserId(ClaimsIdentity identity);
    }
}
