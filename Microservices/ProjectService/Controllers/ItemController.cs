using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Xml;

namespace ProjectService.Controllers;

[ApiController]

public class ItemController(IItemManager itemManager) : ControllerBase
{
    [HttpPost("create")]
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateItemModel item, CancellationToken cancellationToken)
    {
        try
        {
            var id = await itemManager.CreateAsync(item, cancellationToken);
            return Ok(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-current-user-items")]
    public async Task<IActionResult> GetCurrentUserItems()
    {
        try
        {
            var items = await itemManager.GetCurrentUserItems();
            return Ok(items);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("get-user-item/{userId}")]
    public async Task<IActionResult> GetUserItem([FromBody] int projectId, int userId)
    {
        try
        {
            var items = await itemManager.GetItemsByUserId(userId, projectId);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("add-user-to-item/{itemId}")]
    public async Task<IActionResult> AddUserInItem([FromBody] int newUserId, int itemId)
    {
        try
        {
            await itemManager.AddUserToItem(newUserId, itemId);
            return Ok($"Пользователь {newUserId} присоединен к задаче {itemId}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    /// <summary>
    /// Изменение статуса задачи
    /// </summary>
    /// <remarks>
    /// Меняет статус задачи при перемещении её из одной колонки в другую.
    /// </remarks>
    /// <param name="model">ID изменяемой задачи, ID статуса</param>
    [SwaggerOperation("Изменение статуса задачи")]
    [HttpPost("change-status/{itemId}")]
    public async Task<IActionResult> ChangeStatus([FromBody] int statusId, int itemId, CancellationToken cancellationToken)
    {
        //с помощью токена уведомлять о изменении статуса надо сделать
        try
        {
            var itemModel = await itemManager.GetByIdAsync(itemId); 
            itemModel.StatusId = statusId;
            await itemManager.UpdateAsync(itemModel);
            return Ok("Статус обновлён");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("change-itemType/{itemId}")]

    public async Task<IActionResult> ChangeItemType([FromBody] int itemTypeId, int itemId,
        CancellationToken cancellationToken)
    {
        //нужно подумать как обьединить эти 2 метода в один в зависимости от параметров
        try
        {
            var itemModel = await itemManager.GetByIdAsync(itemId); 
            itemModel.ItemTypeId = itemTypeId;
            var newItemModel = await itemManager.UpdateAsync(itemModel);
            return Ok(newItemModel);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("change-params")]//тут передается новая модель но со старым id и ищет в бд по id
                               //запись и меняет в ней все что нужно
    public async Task<IActionResult> ChangeParams([FromBody] ItemModel itemModel, CancellationToken cancellationToken)
    {
        try
        {
            var newItemModel = await itemManager.UpdateAsync(itemModel);
            return Ok(newItemModel);
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
        var items = await itemManager.GetAllItemsAsync();
        return Ok(items);
    }
    
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await itemManager.Delete(id);
        return Ok(id);
    }

    [HttpGet("get/{id}")]
    [ProducesResponseType<ItemModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetItemByIdAsync(int id)
    {
        var item = await itemManager.GetByIdAsync(id);
        return Ok(item);
    }

    [HttpGet("board/{boardId}")]
    public async Task<IActionResult> GetItemsByBoardIdAsync(int boardId)
    {
        var items = await itemManager.GetByBoardIdAsync(boardId);
        return Ok(items);
    }

    [HttpGet("{title}")]
    public async Task<IActionResult> GetItemByName(string title)
    {
        var item = await itemManager.GetByTitle(title);
        return Ok(item);
    }
}