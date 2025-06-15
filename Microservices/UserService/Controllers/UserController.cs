using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Auth;
using SharedLibrary.ProjectModels;
using SharedLibrary.UserModels;
using UserService.BusinessLayer.Manager;
using UserService.DataLayer.Repositories.Abstractions;

namespace UserService.Controllers
{
    [ApiController]
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IAuth auth;
        private readonly IUserManager userManager;

        public UserController(IAuth auth, IUserManager userManager)
        {
            this.auth = auth;
            this.userManager = userManager;
        }

        [HttpGet("get-user-by/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            return Ok(await userManager.GetByIdAsync(userId));
        }

        [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("current/{id}")]
        public async Task<IActionResult> GetCurrentUserInfo(int id)
        {
            var userId = auth.GetCurrentUserId();

            if (userId == id)
                return Ok(await userManager.GetByIdAsync(id));

            return Unauthorized("������� �� ��������������� �������");
        }

        [HttpPost("set-avatar")]
        public async Task<IActionResult> SetUserAvatar(IFormFile avatar)
        {
            if (avatar == null || avatar.Length == 0)
                return BadRequest("���� �� ��������.");

            var userId = auth.GetCurrentUserId();

            if (userId is null || userId == -1)
                return Unauthorized("������� �� ��������������� �������");

            try
            {
                await userManager.SetUserAvatar((int)userId, avatar);
                return Ok("������ �������");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
