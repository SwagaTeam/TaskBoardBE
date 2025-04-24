using Contributors.BusinessLayer.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Contributors.Controllers;

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