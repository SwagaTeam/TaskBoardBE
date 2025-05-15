using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;

namespace ProjectService.Controllers;

[ApiController]
[Route("sprint")]
public class SprintController : ControllerBase
{
    private readonly ISprintManager sprintManager;

    public SprintController(ISprintManager sprintManager)
    {
        this.sprintManager = sprintManager;
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var sprint = await sprintManager.GetByIdAsync(id);
            return Ok(sprint);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get/board/{boardId}")]
    public async Task<IActionResult> GetByBoardId(int boardId)
    {
        try
        {
            var sprints = await sprintManager.GetByBoardIdAsync(boardId);
            return Ok(sprints);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(SprintModel model)
    {
        try
        {
            var result = await sprintManager.CreateAsync(model);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("add-item/{sprintId}")]
    public async Task<IActionResult> AddItemToSprint(int sprintId, int itemId)
    {
        try
        {
            await sprintManager.AddItem(sprintId, itemId);
            return Ok("Задача добавлена в спринт");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("update")]
    public async Task<IActionResult> Update(SprintModel model)
    {
        try
        {
            var result = await sprintManager.UpdateAsync(model);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("delete/{sprintId}")]
    public async Task<IActionResult> Delete(int sprintId)
    {
        try
        {
            await sprintManager.DeleteAsync(sprintId);
            return Ok(sprintId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}