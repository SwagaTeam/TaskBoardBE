using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Services.MailService;
using SharedLibrary.Auth;
using SharedLibrary.Dapper;
using SharedLibrary.Dapper.DapperRepositories;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;
using System;
using System.Reflection.Metadata.Ecma335;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("project")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectManager _projectManager;
        private readonly IProjectLinkManager _projectLinkManager;
        private readonly IEmailSender _emailSender;
        private readonly IAuth _auth;
        public ProjectController(IProjectManager projectManager, IProjectLinkManager projectLinkManager, IEmailSender emailSender, IAuth auth)
        {
            _projectLinkManager = projectLinkManager;
            _projectManager = projectManager;
            _emailSender = emailSender;
            _auth = auth;
        }

        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("send-invite")]
        public async Task<IActionResult> SendInvite([FromBody] InviteRequest request)
        {
            var link = await _projectLinkManager.CreateAsync(request.ProjectId);

            link = $"{Request.Scheme}://{Request.Host}/project/invite/{link}";

            var project = await _projectManager.GetByIdAsync(request.ProjectId);

            var user = await UserRepository.GetUserByEmail (request.Email);

            if (user is null || project is null)
                return BadRequest("������������ ��� ������� �� ����������");

            await _emailSender.SendEmailAsync(
                user.Email, 
                "����������� � ������", 
                $"������������, {user.Username}, ��� ���������� � ������ {project.Name}.\n" +
                $"������-�����������: {link}");

            return Ok(link);
        }

        [HttpGet("invite/{link}")]
        public async Task<IActionResult> GetProjectInfo(string link)
        {
            var projectLink = await _projectLinkManager.GetByLinkAsync(link);

            if (projectLink == null)
                return NotFound();

            return Ok(new { projectLink.ProjectId, projectLink.Project!.Name });
        }

        [HttpPost("invite/{url}/join")]
        //TODO �������� AUTHORIZED
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
                return BadRequest("������������ ��� ������� � �������");

            await _projectManager.AddUserInProjectAsync((int)userId, projectLink.ProjectId);

            return Ok("������������ �������� � ������");
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
            var projectId = await _projectManager.CreateAsync(model);

            return Ok(projectId);
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
            //TODO: �������� Entity �� dto
            return Ok(id);
        }
    }
}
