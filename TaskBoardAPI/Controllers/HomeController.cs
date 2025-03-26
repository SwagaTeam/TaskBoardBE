using Microsoft.AspNetCore.Mvc;

namespace TaskBoardAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class HomeController : ControllerBase
    {

        public HomeController()
        {
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Success");
        }
    }
}
