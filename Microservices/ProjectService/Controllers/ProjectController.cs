using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("project")]
    public class ProjectController : ControllerBase
    {

        public ProjectController()
        {
        }

        [ProducesResponseType<IEnumerable<ProjectModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("get")]
        public async Task<IActionResult> GetAll()
        {
            List<ProjectModel> projects = new List<ProjectModel>();
            return Ok(projects);
        }

        [ProducesResponseType<ProjectModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            ProjectModel project = new ProjectModel();
            return Ok(project);
        }

        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] ProjectModel model)
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
        public async Task<IActionResult> ChangeStatus([FromBody] StatusEntity status, int id)
        {
            //TODO: поменять Entity на dto
            return Ok(id);
        }
    }
}
