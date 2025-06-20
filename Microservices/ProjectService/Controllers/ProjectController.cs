using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;
using ProjectService.Services.MailService;
using SharedLibrary.Auth;
using SharedLibrary.Dapper.DapperRepositories;
using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.MailService;
using SharedLibrary.Models.DocumentModel;
using SharedLibrary.ProjectModels;
using Swashbuckle.AspNetCore.Annotations;

namespace ProjectService.Controllers;

[ApiController]
[Route("project")]
public class ProjectController : ControllerBase
{
    private readonly IProjectManager _projectManager;
    private readonly IProjectLinkManager _projectLinkManager;
    private readonly IEmailSender _emailSender;
    private readonly IAuth _auth;
    private readonly IDocumentManager _documentManager;
    private readonly IUserRepository userRepository;

    public ProjectController(IProjectManager projectManager, IProjectLinkManager projectLinkManager,
        IEmailSender emailSender, IAuth auth, IDocumentManager documentManager, IUserRepository userRepository)
    {
        _projectLinkManager = projectLinkManager;
        _projectManager = projectManager;
        _emailSender = emailSender;
        _auth = auth;
        _documentManager = documentManager;
        this.userRepository = userRepository;
    }

    /// <summary>
    /// Установка роли пользователя в проекте.
    /// </summary>
    /// <param name="model">Модель с данными: UserId, ProjectId и роль пользователя.</param>
    [HttpPost("set-user-role")]
    public async Task<IActionResult> SetUserRole([FromBody] SetUserRoleModel model)

    {
        try
        {
            await _projectManager.SetUserRoleAsync(model.UserId, model.ProjectId, model.Role);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Ok();
    }

    /// <summary>
    /// Отправка приглашения в проект по электронной почте.
    /// </summary>
    /// <remarks>
    /// Генерируется уникальная ссылка-приглашение и отправляется на указанный email.
    /// </remarks>
    /// <param name="request">Модель с данными: ProjectId и Email пользователя для приглашения.</param>
    /// <returns>Ссылка-приглашение на присоединение к проекту.</returns>
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpPost("send-invite")]
    public async Task<IActionResult> SendInvite([FromBody] InviteRequest request)

    {
        try
        {
            var project = await _projectManager.GetByIdAsync(request.ProjectId);

            var user = await userRepository.GetUserByEmailAsync(request.Email);

            if (user is null || project is null)
                return BadRequest("Такого пользователя или проекта не существует");
            
            var link = await _projectLinkManager.CreateAsync(request.ProjectId, user.Id);

            link = $"https://boardly.ru/project/invite/{link}";
            await _emailSender.SendEmailAsync(
                "Приглашение в проект",
                $"Здравствуйте, {user.Username}, Вас приглашают в проект {project.Name}.\n" +
                $"Вы можете принять приглашение по ссылке: {link}", to:user.Email);

            return Ok(link);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Получение всех пользователей в проекте по projectId
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    [HttpGet("get-users-in-project/{projectId}")]
    public async Task<IActionResult> GetUsersInProject(int projectId)
    {
        var users = await _projectManager.GetUsersInProjectAsync(projectId);
        if (users is null) return NotFound("не найден проект или список участников пуст");
        return Ok(users);
    }

    /// <summary>
    /// Получение информации о проекте по ссылке-приглашению.
    /// </summary>
    /// <param name="link">Уникальная ссылка-приглашение.</param>
    /// <returns>Данные проекта: ID и название.</returns>
    [HttpGet("invite/{link}")]
    public async Task<IActionResult> GetProjectInfo(string link)
    {
        var projectLink = await _projectLinkManager.GetByLinkAsync(link);

        if (projectLink == null)
            return NotFound();

        return Ok(new { projectLink.ProjectId, projectLink.Project!.Name });
    }

    /// <summary>
    /// Присоединение пользователя к проекту по ссылке-приглашению.
    /// </summary>
    /// <param name="url">Уникальная ссылка-приглашение.</param>
    /// <returns>Статус успешного присоединения.</returns>
    [HttpPost("invite/{url}/join")]
    public async Task<IActionResult> JoinProject(string url)
    {
        var userId = _auth.GetCurrentUserId();

        if (userId == -1 || userId is null)
            return Unauthorized();

        var projectLink = await _projectLinkManager.GetByLinkAsync(url);

        if (projectLink == null)
            return NotFound();

        var alreadyIn = await _projectManager.IsUserInProjectAsync((int)userId, projectLink.ProjectId);

        if (alreadyIn)
            return BadRequest("Вы уже находитесь в проекте");
        
        if (projectLink.UserId != userId) return BadRequest("Эта ссылка-приглашение для другого пользователя");

        await _projectManager.AddUserInProjectAsync((int)userId, projectLink.ProjectId);

        return Ok($"Вы успешно присоединились к проекту");
    }

    /// <summary>
    /// Получение списка всех проектов текущего пользователя.
    /// </summary>
    /// <returns>Список проектов пользователя.</returns>
    [ProducesResponseType<IEnumerable<ProjectModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var projects = await _projectManager.Get();
            return Ok(projects);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обзор состояния задач на странице проекта.
    /// </summary>
    [ProducesResponseType<TasksState>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("get-tasks-state/{projectId}")]
    public async Task<IActionResult> GetTasksState(int projectId)
    {
        try
        {
            var state = await _projectManager.GetTasksStateAsync(projectId);
            return Ok(state);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Получение проекта по ID, если есть доступ.
    /// </summary>
    /// <returns>Модель Проекта.</returns>
    [ProducesResponseType<ProjectModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [HttpGet("get/{projectId}")]
    public async Task<IActionResult> GetById(int projectId)
    {
        try
        {
            var project = await _projectManager.GetByIdAsync(projectId);
            return Ok(project);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Получение проекта по ID.
    /// </summary>
    /// <param name="id">ID проекта.</param>
    /// <returns>Данные проекта.</returns>
    [ProducesResponseType<ProjectModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            var project = await _projectManager.GetByIdAsync(id);
            return Ok(project);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Создание нового проекта.
    /// </summary>
    /// <remarks>
    ///     <br /><br />
    ///     <b>Статусы (Priority):</b>
    ///     <ul>
    ///         <li>0 – Не активен</li>
    ///         <li>1 – В работе</li>
    ///         <li>2 – Завершён</li>
    ///     </ul>
    /// </remarks>
    /// <param name="model">Модель проекта с необходимыми данными.</param>
    /// <returns>ID созданного проекта.</returns>
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ProjectModel model)
    {
        var projectId = await _projectManager.CreateAsync(model);

        return Ok(projectId);
    }

    /// <summary>
    /// Прикрепление документа к проекту.
    /// </summary>
    /// <param name="document">Файл документа.</param>
    /// <param name="projectId">ID проекта, к которому прикрепляется документ.</param>
    [SwaggerOperation("Прикрепление документа к проекту")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("attach-document/{projectId}")]
    public async Task<IActionResult> AttachDocument(IFormFile document, int projectId)
    {
        if (document == null || document.Length == 0)
            return BadRequest("Файл не загружен.");

        try
        {
            await _documentManager.AttachDocument(document, projectId);
            return Ok("Документ прикреплён");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Получение списка документов проекта.
    /// </summary>
    /// <param name="projectId">ID проекта.</param>
    /// <returns>Список документов, прикреплённых к проекту.</returns>
    [SwaggerOperation("Получение документов проекта")]
    [ProducesResponseType<IEnumerable<DocumentModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("get-project-documents/{projectId}")]
    public async Task<IActionResult> GetDocuments(int projectId)
    {
        try
        {
            var docs = await _documentManager.GetByProjectIdAsync(projectId);
            return Ok(docs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Удаление проекта по ID.
    /// </summary>
    /// <param name="id">ID проекта для удаления.</param>
    /// <returns>ID удалённого проекта.</returns>
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)

    {
        try
        {
            await _projectManager.DeleteAsync(id);
            return Ok(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    ///     Изменение статуса проекта.
    /// </summary>
    /// ///
    /// <remarks>
    ///     <br /><br />
    ///     <b>Статусы:</b>
    ///     <ul>
    ///         <li>0 – Не активен</li>
    ///         <li>1 – В работе</li>
    ///         <li>2 – Завершён</li>
    ///     </ul>
    /// </remarks>
    /// <param name="status">Новый статус</param>
    /// <param name="projectId">ID проекта</param>
    [SwaggerOperation("Изменение статуса проекта ")]
    [HttpPost("change-status/{projectId}")]
    public async Task<IActionResult> ChangePriority([FromBody] int status, int projectId,
        CancellationToken cancellationToken)
    {
        try
        {
            var projectModel = await _projectManager.GetByIdAsync(projectId);
            projectModel.Priority = status;
            var newItemModel = await _projectManager.UpdateAsync(projectModel);
            return Ok(newItemModel);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    ///     Изменение параметров проекта.
    /// </summary>
    /// <remarks>
    ///     Заменяет все параметры проекта на новые, кроме ID.
    /// </remarks>
    /// <param name="projectModel">Модель проекта с изменёнными параметрами</param>
    [HttpPost("change-params")]
    public async Task<IActionResult> ChangeParams([FromBody] ProjectModel projectModel, CancellationToken cancellationToken)
    {
        try
        {
            var newItemModel = await _projectManager.UpdateAsync(projectModel);
            return Ok(newItemModel);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
