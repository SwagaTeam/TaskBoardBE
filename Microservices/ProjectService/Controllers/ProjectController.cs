using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;
using ProjectService.Services.MailService;
using SharedLibrary.Auth;
using SharedLibrary.Dapper;
using SharedLibrary.Dapper.DapperRepositories;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models.DocumentModel;
using SharedLibrary.ProjectModels;
using Swashbuckle.AspNetCore.Annotations;
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
        private readonly IDocumentManager _documentManager;
        public ProjectController(IProjectManager projectManager, IProjectLinkManager projectLinkManager, IEmailSender emailSender, IAuth auth, IDocumentManager documentManager)
        {
            _projectLinkManager = projectLinkManager;
            _projectManager = projectManager;
            _emailSender = emailSender;
            _auth = auth;
            _documentManager = documentManager;
        }

        [HttpPost("set-user-role")]
        public async Task<IActionResult> SetUserRole([FromBody] SetUserRoleModel model)
        {
            try 
            {
                await _projectManager.SetUserRoleAsync(model.UserId, model.ProjectId, model.Role);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("send-invite")]
        public async Task<IActionResult> SendInvite([FromBody] InviteRequest request)
        {
            try
            {
                var link = await _projectLinkManager.CreateAsync(request.ProjectId);

                link = $"{Request.Scheme}://{Request.Host}/project/invite/{link}";

                var project = await _projectManager.GetByIdAsync(request.ProjectId);

                var user = await UserRepository.GetUserByEmail(request.Email);

                if (user is null || project is null)
                    return BadRequest("������������ ��� ������� �� ����������");

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "����������� � ������",
                    $"������������, {user.Username}, ��� ���������� � ������ {project.Name}.\n" +
                    $"������-�����������: {link}");

                return Ok(link);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
        /// <param name="projectId">ID проекта, к которому прикрепляется документ </param>
        [SwaggerOperation("Прикрепление документа к проекту")]
        [ProducesResponseType<string>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("attach-document/{projectId}")]
        public async Task<IActionResult> Create(IFormFile document, int projectId)
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
        /// Получение документов проекта.
        /// </summary>
        /// <param name="projectId">ID проекта</param>
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


        [ProducesResponseType<int>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("get/{id}")]
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
    }
}
