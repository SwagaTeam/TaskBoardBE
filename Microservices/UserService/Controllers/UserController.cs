using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Constants;
using SharedLibrary.ProjectModels;
using SharedLibrary.UserModels;

namespace UserService.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {

        [ProducesResponseType<IEnumerable<UserModel>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("get")]
        public async Task<IActionResult> GetAll()
        {
            List<UserModel> users = new List<UserModel>();
            return Ok(users);
        }

        [ProducesResponseType<TaskModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            UserModel user = new UserModel();
            return Ok(user);
        }
    }
}
