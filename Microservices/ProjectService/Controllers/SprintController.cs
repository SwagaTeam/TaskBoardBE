using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("sprint")]
    public class SprintController : ControllerBase
    {
        private readonly ISprintManager sprintManager;
        public SprintController(ISprintManager sprintManager)
        {
            this.sprintManager = sprintManager;
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var sprint = await sprintManager.GetByIdAsync(id);
                return Ok(sprint);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/board/{boardId}")]
        public async Task<IActionResult> GetByBoardId(int boardId)
        {
            try
            {
                var sprints = await sprintManager.GetByBoardIdAsync(boardId);
                return Ok(sprints);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(SprintModel model)
        {
            try
            {
                var result = await sprintManager.CreateAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update(SprintModel model)
        {
            try
            {
                var result = await sprintManager.UpdateAsync(model);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
