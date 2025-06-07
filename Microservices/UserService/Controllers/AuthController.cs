using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Validations;
using SharedLibrary.Auth;
using SharedLibrary.ProjectModels;
using SharedLibrary.UserModels;
using System.Text.Json;
using UserService.BusinessLayer.Manager;
using UserService.ViewModels;

namespace UserService.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuth auth;
        private readonly IUserManager userManager;
        private readonly ILogger<AuthController> logger;

        public AuthController(IAuth auth, IUserManager userManager, ILogger<AuthController> logger)
        {
            this.auth = auth;
            this.userManager = userManager;
            this.logger = logger;

        }

        [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = auth.GetCurrentUserId();
                if (userId == -1) return Unauthorized("Не авторизован");
                var roles = auth.GetCurrentUserRoles();
                logger.LogInformation($"INFO: User with id \"{userId}\" recieved his own id and roles.");
                return Ok(new { userId, roles });
            }
            catch (Exception ex)
            {
                logger.LogError($"\nREQUEST FAILED: Failed get user data." +
                                      $"\nMessage: {ex.Message}\n");
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            try
            {
                var result = await userManager.Create(model);
                logger.LogInformation($"REGISTER: A new User with id \"{result}\" has been created.");
                return Ok(result);
            }
            catch (Exception ex)
            {
                logger.LogError($"\nREGISTER FAILED: Failed register subscriber." +
                                      $"\nMessage: {ex.Message}" +
                                      $"\nModel: {JsonSerializer.Serialize(model)}\n");
                return BadRequest("Ошибка при регистрации пользователя");
            }
        }

        [ProducesResponseType<JsonProperty>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            try
            {
                if (auth.GetCurrentUserId() != -1)
                    throw new Exception($"Вы уже авторизованы");
                var user = await userManager.ValidateCredentials(loginModel.Email, loginModel.Password);
                if (user == null)
                    return Unauthorized("Неверный номер/пароль");
                var token = auth.GenerateJwtToken(user);
                logger.LogInformation($"LOGIN: User with id \"{user.Id}\" has been authorized.");
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                logger.LogError($"LOGIN FAILED: Failed subscriber login.");
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType<JsonProperty>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("change-password")]
        public async Task<IActionResult> СhangePassword([FromBody] ChangePasswordModel changePasswordModel)
        {
            try
            {
                var userId = auth.GetCurrentUserId();
                if (userId == -1)
                    throw new Exception($"Вы не авторизованы");

                var user = await userManager.GetById((int)userId);

                var validatedUser = await userManager.ValidateCredentials(user.Email, changePasswordModel.LastPassword);
                if (validatedUser == null)
                    return Unauthorized("Неверный пароль");

                await userManager.ChangePassword((int)userId, changePasswordModel.NewPassword);

                var token = auth.GenerateJwtToken(user);

                logger.LogInformation($"LOGIN: User with id \"{user.Id}\" has been authorized.");

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                logger.LogError($"LOGIN FAILED: Failed subscriber login.");
                return BadRequest(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Пустой токен.");
            }

            auth.Logout(token);
            return Ok("Успешный выход из системы.");
        }
    }
}
