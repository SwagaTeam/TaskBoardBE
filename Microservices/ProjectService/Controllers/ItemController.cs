using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;

namespace ProjectService.Controllers;

[ApiController]

public class ItemController(IItemManager manager) : ControllerBase
{
    [HttpPost("create")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateItemModel item, CancellationToken cancellationToken)
    {
        try
        {
            var id = await manager.CreateAsync(item, cancellationToken);
            return Ok(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get")]
    [ProducesResponseType<IEnumerable<ItemModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetItemsAsync()
    {
        var items = await manager.GetAllItemsAsync();
        return Ok(items);
    }
    
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await manager.Delete(id);
        return Ok(id);
    }

    [HttpGet("{id}")]
    [ProducesResponseType<ItemModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetItemByIdAsync(int id)
    {
        var item = await manager.GetByIdAsync(id);
        return Ok(item);
    }

    [HttpGet("{title}")]
    public async Task<IActionResult> GetItemByName(string title)
    {
        var item = await manager.GetByTitle(title);
        return Ok(item);
    }
}