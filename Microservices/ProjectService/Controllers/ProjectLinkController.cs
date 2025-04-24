using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("project-link")]
    public class ProjectLinkController(IProjectLinkManager projectLinkManager, IProjectManager projectManager)
        : ControllerBase
    {
        private readonly IProjectLinkManager _projectLinkManager = projectLinkManager;
        private readonly IProjectManager _projectManager = projectManager;
    }
}
