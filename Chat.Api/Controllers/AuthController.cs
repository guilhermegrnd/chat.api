using Fullstack.API.Interfaces;
using Fullstack.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Fullstack.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IAuth _auth;

        public AuthController(ILogger<AuthController> logger, IAuth auth)
        {
            _auth = auth;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Authenticate(User request)
        {
            var defaultResult = new DefaultResult<Auth>();
            try
            {
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/login started run.");
                defaultResult.Data = await _auth.Authenticate(request);
                defaultResult.Success = true;
                defaultResult.Message = "Authenticated Successfully";
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/login ended run.");

                return StatusCode((int)HttpStatusCode.OK, defaultResult);
            }
            catch (Exception ex)
            {
                defaultResult.Message += $"Some server error has happened. Call the system administrator.";
                _logger.LogError(ex, "An error occurred");
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/login ended run with system error.");
                return StatusCode((int)HttpStatusCode.InternalServerError, defaultResult);
            }
        }

        [Route("refresh")]
        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] Auth request)
        {
            var defaultResult = new DefaultResult<Auth>();
            try
            {
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/refresh started run.");
                defaultResult.Data = await _auth.RefreshTokenAsync(request);
                defaultResult.Success = true;
                defaultResult.Message = "Token Refreshed Successfully";
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/refresh ended run.");

                return StatusCode((int)HttpStatusCode.OK, defaultResult);
            }
            catch (Exception ex)
            {
                defaultResult.Message += $"Some server error has happened. Call the system administrator.";
                _logger.LogError(ex, "An error occurred");
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/refresh ended run with system error.");
                return StatusCode((int)HttpStatusCode.InternalServerError, defaultResult);
            }
        }

        [Route("logout")]
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Logout()
        {
            var defaultResult = new DefaultResult<bool>();
            try
            {
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/logout started run.");
                defaultResult.Data = await _auth.DeleteRefreshToken(HttpContext.User.Identity as ClaimsIdentity);
                defaultResult.Success = true;
                defaultResult.Message = "Logged Out Successfully";
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/logout ended run.");

                return StatusCode((int)HttpStatusCode.OK, defaultResult);
            }
            catch (Exception ex)
            {
                defaultResult.Message += $"Some server error has happened. Call the system administrator.";
                _logger.LogError(ex, "An error occurred");
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/logout ended run with system error.");
                return StatusCode((int)HttpStatusCode.InternalServerError, defaultResult);
            }
        }
    }
}
