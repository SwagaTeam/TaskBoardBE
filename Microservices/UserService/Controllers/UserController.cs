using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {

        public UserController()
        {
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
