using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {

        public AuthController()
        {
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
