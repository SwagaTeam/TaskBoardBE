using Microsoft.AspNetCore.Mvc;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("project")]
    public class ProjectController : ControllerBase
    {

        public ProjectController()
        {
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok("project");
        }
    }
}
