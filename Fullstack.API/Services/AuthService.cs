using Fullstack.API.Data;
using Fullstack.API.Interfaces;
using Fullstack.API.Models;
using Fullstack.API.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Transactions;

namespace Fullstack.API.Services
{
    public class AuthService : IAuth
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public AuthService(IConfiguration configuration, DataContext context, TokenValidationParameters tokenValidationParameters)
        {
            _configuration = configuration;
            _context = context;
            _tokenValidationParameters = tokenValidationParameters;
        }
        public async Task<Auth> Authenticate(User user, bool alreadyHashed = false)
        {
            string passwordHashed = PasswordHasher.Hash(user.Password);

            if (alreadyHashed) passwordHashed = user.Password;

            var userContext = _context.Users
                .Where(x =>
                    x.Name == user.Name
                    && x.Password == passwordHashed
                )
                .FirstOrDefault();

            if (userContext == null)
            {
                throw new Exception("Invalid Login and/or Password");
            }

            // Else we generate JSON Web Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenHandlerRefresh = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", userContext.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),//DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userContext.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new Auth { Token = tokenHandler.WriteToken(token), RefreshToken = refreshToken.Token };
        }

        public async Task<Auth> RefreshTokenAsync(Auth request)
        {
            return await GetRefreshTokenAsync(request.Token, request.RefreshToken);
        }

        private async Task<Auth> GetRefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                //return new AuthenticationResult { Errors = new[] { "Invalid Token" } };
                throw new Exception("Invalid Token");
            }

            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                //return new AuthenticationResult { Errors = new[] { "This token hasn't expired yet" } };
                throw new Exception("This token hasn't expired yet");
            }

            //var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                //return new AuthenticationResult { Errors = new[] { "This refresh token does not exist" } };
                throw new Exception("This refresh token does not exist");
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                //return new AuthenticationResult { Errors = new[] { "This refresh token has expired" } };
                throw new Exception("This refresh token has expired");
            }

            //if (storedRefreshToken.JwtId != jti)
            //{
            //return new AuthenticationResult { Errors = new[] { "This refresh token does not match this JWT" } };
            //    return null;
            //

            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();
            string strUserId = validatedToken.Claims.Single(x => x.Type == "UserId").Value;
            long userId = 0;
            long.TryParse(strUserId, out userId);
            var user = _context.Users.Where(u => u.Id == userId).FirstOrDefault();
            if (user == null)
            {
                //return new AuthenticationResult { Errors = new[] { "User Not Found" } };
                throw new Exception("User Not Found");
            }

            return await Authenticate(user, true);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }

        public async Task<bool> DeleteRefreshToken(ClaimsIdentity identity)
        {
            long UserId = await GetAuthenticatedUserId(identity);

            using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                var refreshTokens = _context.RefreshTokens
                    .Where(x => x.UserId == UserId)
                    .ToList();
                _context.RefreshTokens.RemoveRange(refreshTokens);
                await _context.SaveChangesAsync();

                scope.Complete();
            }

            return true;
        }

        public async Task<long> GetAuthenticatedUserId(ClaimsIdentity identity)
        {
            if (identity == null) throw new Exception("Identity Not Found");

            //if (!identity.IsAuthenticated) throw new Exception("Not Authenticated");

            string strUserId = identity.FindFirst("UserId").Value;

            return long.Parse(strUserId);
        }
    }
}
