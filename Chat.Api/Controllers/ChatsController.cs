using Chat.API.Interfaces;
using Chat.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Chat.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatsController : Controller
    {
        private readonly ILogger<ChatsController> _logger;
        private readonly IChat _chats;
        private readonly IAuth _auth;

        public ChatsController(ILogger<ChatsController> logger, IChat chats, IAuth auth)
        {
            _chats = chats;
            _logger = logger;
            _auth = auth;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllChats()
        {
            var defaultResult = new DefaultResult<List<User>>();
            try
            {
                _logger.LogInformation($"Endpoint GET api/v1/[controller] started run.");
                defaultResult.Data = await _chats.GetChats(await _auth.GetAuthenticatedUserId(HttpContext.User.Identity as ClaimsIdentity));
                defaultResult.Success = true;
                defaultResult.Message = "Data Retrieved Successfully";
                _logger.LogInformation($"Endpoint GET api/v1/[controller] ended run.");

                return StatusCode((int)HttpStatusCode.OK, defaultResult);
            }
            catch (Exception ex)
            {
                defaultResult.Message += $"Some server error has happened. Call the system administrator.";
                _logger.LogError(ex, "An error occurred");
                _logger.LogInformation($"Endpoint GET api/v1/[controller] ended run with system error.");
                return StatusCode((int)HttpStatusCode.InternalServerError, defaultResult);
            }
        }

        [HttpGet]
        [Route("{toUserId:long}/messages")]
        public async Task<IActionResult> GetChatMessagess([FromRoute] long toUserId)
        {
            var defaultResult = new DefaultResult<List<Message>>();
            try
            {
                _logger.LogInformation($"Endpoint GET api/v1/[controller]/{toUserId}/messages started run.");
                defaultResult.Data = await _chats.GetChatMessages(await _auth.GetAuthenticatedUserId(HttpContext.User.Identity as ClaimsIdentity), toUserId);
                defaultResult.Success = true;
                defaultResult.Message = "Data Retrieved Successfully";
                _logger.LogInformation($"Endpoint GET api/v1/[controller]/{toUserId}/messages ended run.");

                return StatusCode((int)HttpStatusCode.OK, defaultResult);
            }
            catch (Exception ex)
            {
                defaultResult.Message += $"Some server error has happened. Call the system administrator.";
                _logger.LogError(ex, "An error occurred");
                _logger.LogInformation($"Endpoint GET api/v1/[controller]/{toUserId}/messages ended run with system error.");
                return StatusCode((int)HttpStatusCode.InternalServerError, defaultResult);
            }
        }

        [HttpPost]
        [Route("{toUserId:long}/messages")]
        public async Task<IActionResult> AddMessage([FromRoute] long toUserId, [FromBody] Message messageRequest)
        {
            var defaultResult = new DefaultResult<Message>();
            try
            {
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/{toUserId}/messages started run.");
                defaultResult.Data = await _chats.AddMessage(await _auth.GetAuthenticatedUserId(HttpContext.User.Identity as ClaimsIdentity), toUserId, messageRequest);
                defaultResult.Success = true;
                defaultResult.Message = "Data Updated Successfully";
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/{toUserId}/messages ended run.");

                return StatusCode((int)HttpStatusCode.Created, defaultResult);
            }
            catch (Exception ex)
            {
                defaultResult.Message += $"Some server error has happened. Call the system administrator.";
                _logger.LogError(ex, "An error occurred");
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/{toUserId}/messages ended run with system error.");
                return StatusCode((int)HttpStatusCode.InternalServerError, defaultResult);
            }
        }

        [HttpDelete]
        [Route("{toUserId:long}/messages/{messageId:long}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteMessage([FromRoute] long toUserId, [FromRoute] long messageId)
        {
            var defaultResult = new DefaultResult<bool>();
            try
            {
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/{toUserId}/messages/{messageId} started run.");
                defaultResult.Data = await _chats.DeleteMessage(await _auth.GetAuthenticatedUserId(HttpContext.User.Identity as ClaimsIdentity), toUserId, messageId);
                defaultResult.Success = true;
                defaultResult.Message = "Data Updated Successfully";
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/{toUserId}/messages/{messageId} ended run.");

                return StatusCode((int)HttpStatusCode.OK, defaultResult);
            }
            catch (Exception ex)
            {
                defaultResult.Message += $"Some server error has happened. Call the system administrator.";
                _logger.LogError(ex, "An error occurred");
                _logger.LogInformation($"Endpoint POST api/v1/[controller]/{toUserId}/messages/{messageId} ended run with system error.");
                return StatusCode((int)HttpStatusCode.InternalServerError, defaultResult);
            }
        }
    }
}
