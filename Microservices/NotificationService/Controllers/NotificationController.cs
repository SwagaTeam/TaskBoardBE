using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("notification")]
    public class NotificationController : ControllerBase
    {
        [HttpPost("new")]
        public async Task<string> NewNotification([FromBody] string text)
        {
            return text;
        }
    }
}
