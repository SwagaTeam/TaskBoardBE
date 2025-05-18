using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;

namespace ProjectService.Controllers;

[Route("contributors")]
public class ContributorsController(IContributorsManager contributorsManager) : ControllerBase
{
    [HttpGet("get-contributors/{projectId}")]
    public async Task<IActionResult> GetContributors(int projectId)
    {
        var contributors = await contributorsManager.GetUserByProjectIdAsync(projectId);
        return Ok(contributors);
    }
}