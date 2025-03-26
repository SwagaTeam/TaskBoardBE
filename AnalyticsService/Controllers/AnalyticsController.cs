using Microsoft.AspNetCore.Mvc;

namespace AnalyticsService.Controllers
{
    [ApiController]
    [Route("analytics")]
    public class AnalyticsController : ControllerBase
    {

        public AnalyticsController()
        {

        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
