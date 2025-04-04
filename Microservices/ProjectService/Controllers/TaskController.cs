using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Constants;
using SharedLibrary.ProjectModels;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("task")]
    public class TaskController : ControllerBase
    {
        public TaskController()
        {
            
        }

        [ProducesResponseType<IEnumerable<TaskModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("get")]
        public async Task<IActionResult> GetAll()
        {
            List<TaskModel> tasks = new List<TaskModel>();
            return Ok(tasks);
        }

        [ProducesResponseType<TaskModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            TaskModel task = new TaskModel();
            return Ok(task);
        }

        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] TaskModel model)
        {
            return Ok(model.Id);
        }

        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(id);
        }

        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("status/change/{id}")]
        public async Task<IActionResult> ChangeStatus([FromBody] Status status, int id)
        {
            return Ok(id);
        }
    }
}
