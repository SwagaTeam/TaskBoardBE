using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;
using System.Xml;

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

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserItems(int userId)
    {
        try
        {
            var items = await manager.GetItemsByUserId(userId);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("add-user-to-item")]
    public async Task<IActionResult> AddUserInItem(AddUserInItemModel model)
    {
        try
        {
            await manager.AddUserToItem(model.UserId, model.ItemId);
            return Ok($"Пользователь {model.UserId} присоединен к задаче {model.ItemId}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("change-status/{statusId}")]
    public async Task<IActionResult> ChangeStatus([FromBody] ItemModel itemModel, int statusId, CancellationToken cancellationToken)
    {
        //с помощью токена уведомлять о изменении статуса надо сделать
        try
        {
            itemModel.StatusId = statusId;
            var newItemModel = await manager.ChangeParam(itemModel);
            return Ok(newItemModel);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("change-itemTyp/{itemTypeId}")]

    public async Task<IActionResult> ChangeItemType([FromBody] ItemModel itemModel, int itemTypeId,
        CancellationToken cancellationToken)
    {
        //нужно подумать как обьединить эти 2 метода в один в зависимости от параметров
        try
        {
            itemModel.ItemTypeId = itemTypeId;
            var newItemModel = await manager.ChangeParam(itemModel);
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
            var newItemModel = await manager.ChangeParam(itemModel);
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

    [HttpGet("board/{boardId}")]
    public async Task<IActionResult> GetItemsByBoardIdAsync(int boardId)
    {
        var items = await manager.GetByBoardIdAsync(boardId);
        return Ok(items);
    }

    [HttpGet("{title}")]
    public async Task<IActionResult> GetItemByName(string title)
    {
        var item = await manager.GetByTitle(title);
        return Ok(item);
    }
}