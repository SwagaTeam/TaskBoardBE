using Microsoft.AspNetCore.Mvc;

namespace TaskBoardAPI.Controllers
{
    [ApiController]
    [Route("api/home")]
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
