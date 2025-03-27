using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedLibrary.ProjectModels;
using SharedLibrary.UserModels;

namespace UserService.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("current")]
        public IActionResult GetCurrentUser()
        {
            var user = new UserModel();
            return Ok(user);
        }

        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserModel model)
        {
            var user = new UserModel();
            return Ok(user.Id);
        }

        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("login")]
        public IActionResult Login(string login, string password)
        {
            var user = new UserModel();
            return Ok(user.Id);
        }
    }
}
