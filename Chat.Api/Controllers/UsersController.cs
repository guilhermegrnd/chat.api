using Chat.API.Interfaces;
using Chat.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Chat.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class UsersController : Controller
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUsers _users;

        public UsersController(ILogger<UsersController> logger, IUsers users)
        {
            _users = users;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllUsers()
        {
            var defaultResult = new DefaultResult<List<User>>();
            try
            {
                _logger.LogInformation($"Endpoint GET api/v1/[controller] started run.");
                defaultResult.Data = await _users.GetAll();
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

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] User userRequest)
        {
            var defaultResult = new DefaultResult<User>();
            try
            {
                _logger.LogInformation($"Endpoint POST api/v1/[controller] started run.");
                defaultResult.Data = await _users.Add(userRequest);
                defaultResult.Success = true;
                defaultResult.Message = "Data Retrieved Successfully";
                _logger.LogInformation($"Endpoint POST api/v1/[controller] ended run.");

                return StatusCode((int)HttpStatusCode.Created, defaultResult);
            }
            catch (Exception ex)
            {
                defaultResult.Message += $"Some server error has happened. Call the system administrator.";
                _logger.LogError(ex, "An error occurred");
                _logger.LogInformation($"Endpoint POST api/v1/[controller] ended run with system error.");
                return StatusCode((int)HttpStatusCode.InternalServerError, defaultResult);
            }
        }

        [HttpGet]
        [Route("{id:long}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetUser([FromRoute] long id)
        {
            var defaultResult = new DefaultResult<User>();
            try
            {
                _logger.LogInformation($"Endpoint GET api/v1/[controller]/{id} started run.");
                defaultResult.Data = await _users.Get(id);
                defaultResult.Success = true;
                defaultResult.Message = "Data Retrieved Successfully";
                _logger.LogInformation($"Endpoint GET api/v1/[controller]/{id} ended run.");

                return StatusCode((int)HttpStatusCode.OK, defaultResult);
            }
            catch (Exception ex)
            {
                defaultResult.Message += $"Some server error has happened. Call the system administrator.";
                _logger.LogError(ex, "An error occurred");
                _logger.LogInformation($"Endpoint GET api/v1/[controller]/{id} ended run with system error.");
                return StatusCode((int)HttpStatusCode.InternalServerError, defaultResult);
            }
        }

        [HttpPut]
        [Route("{id:long}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUser([FromRoute] long id, [FromBody] User userRequest)
        {
            var defaultResult = new DefaultResult<User>();
            try
            {
                _logger.LogInformation($"Endpoint PUT api/v1/[controller]/{id} started run.");
                defaultResult.Data = await _users.Update(userRequest, id);
                defaultResult.Success = true;
                defaultResult.Message = "Data Updated Successfully";
                _logger.LogInformation($"Endpoint PUT api/v1/[controller]/{id} ended run.");

                return StatusCode((int)HttpStatusCode.OK, defaultResult);
            }
            catch (Exception ex)
            {
                defaultResult.Message += $"Some server error has happened. Call the system administrator.";
                _logger.LogError(ex, "An error occurred");
                _logger.LogInformation($"Endpoint PUT api/v1/[controller]/{id} ended run with system error.");
                return StatusCode((int)HttpStatusCode.InternalServerError, defaultResult);
            }
        }

        [HttpDelete]
        [Route("{id:long}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteUser([FromRoute] long id)
        {
            var defaultResult = new DefaultResult<bool>();
            try
            {
                _logger.LogInformation($"Endpoint DELETE api/v1/[controller]/{id} started run.");
                defaultResult.Data = await _users.Delete(id);
                defaultResult.Success = true;
                defaultResult.Message = "Data Deleted Successfully";
                _logger.LogInformation($"Endpoint DELETE api/v1/[controller]/{id} ended run.");

                return StatusCode((int)HttpStatusCode.OK, defaultResult);
            }
            catch (Exception ex)
            {
                defaultResult.Message += $"Some server error has happened. Call the system administrator.";
                _logger.LogError(ex, "An error occurred");
                _logger.LogInformation($"Endpoint DELETE api/v1/[controller]/{id} ended run with system error.");
                return StatusCode((int)HttpStatusCode.InternalServerError, defaultResult);
            }
        }
    }
}
