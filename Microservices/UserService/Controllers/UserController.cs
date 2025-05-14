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
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IAuth auth;
        private readonly IUserManager userManager;

        public UserController(IAuth auth, IUserManager userManager)
        {
            this.auth = auth;
            this.userManager = userManager;
        }

        [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("current/{id}")]
        public async Task<IActionResult> GetCurrentUserInfo(int id)
        {
            var userId = auth.GetCurrentUserId();

            if (userId == id)
                return Ok(await userManager.GetById(id));

            return Unauthorized("Попытка не авторизованного доступа");
        }

        [HttpPost("set-avatar")]
        public async Task<IActionResult> SetUserAvatar(IFormFile avatar)
        {
            if (avatar == null || avatar.Length == 0)
                return BadRequest("Файл не загружен.");

            var userId = auth.GetCurrentUserId();

            if (userId is null || userId == -1)
                return Unauthorized("Попытка не авторизованного доступа");

            try
            {
                await userManager.SetUserAvatar((int)userId, avatar);
                return Ok("Аватар обновлён");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
