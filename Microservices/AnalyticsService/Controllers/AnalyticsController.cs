using Microsoft.AspNetCore.Mvc;
using SharedLibrary.ProjectModels;

namespace AnalyticsService.Controllers
{
    [ApiController]
    [Route("analytics")]
    public class AnalyticsController : ControllerBase
    {

        public AnalyticsController()
        {

        }

        [ProducesResponseType<TimeSpan>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("time")]
        public async Task<IActionResult> GetAvgTime()
        {
            TimeSpan time = new TimeSpan(1000);
            return Ok(time);
        }

        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("speed")]
        public async Task<IActionResult> GetTeamSpeed()
        {
            int tasksCount = 40;
            int daysCount = 7;
            string speed = $"{tasksCount} tasks per {daysCount} days";
            return Ok(speed);
        }

        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("ratio")]
        public async Task<IActionResult> GetSprintRatio()
        {
            int completedSprints = 10;
            int allSprints = 15;
            int ratio = completedSprints / allSprints;
            return Ok(ratio);
        }
    }
}
