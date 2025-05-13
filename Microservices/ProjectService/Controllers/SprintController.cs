using Microsoft.AspNetCore.Mvc;
using ProjectService.Models;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("sprint")]
    public class SprintController : ControllerBase
    {
        [HttpGet("get")]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok();
        }

        [HttpGet("get/board/{boardId}")]
        public async Task<IActionResult> GetByBoardId(int boardId)
        {
            return Ok();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(SprintModel model)
        {
            return Ok();
        }

        [HttpPost("update-period")]
        public async Task<IActionResult> UpdatePeriod(UpdatePeriodModel model)
        {
            return Ok();
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(SprintModel model)
        {
            return Ok();
        }
    }
}
