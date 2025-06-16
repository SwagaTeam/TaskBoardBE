using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;

namespace ProjectService.Controllers;

[ApiController]
[Route("statuses")]
public class StatusController(IStatusManager manager) : ControllerBase
{
    [HttpGet("get-all")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var statuses = await manager.GetAllAsync();
            return Ok(statuses);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}