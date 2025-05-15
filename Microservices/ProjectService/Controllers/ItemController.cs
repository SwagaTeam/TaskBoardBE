using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Xml;

namespace ProjectService.Controllers;

[ApiController]

public class ItemController(IItemManager itemManager) : ControllerBase
{
    /// <summary>
    /// Добавление новой задачи/эпика/бага.
    /// </summary>
    /// <remarks>
    /// Этот метод добавляет новую задачу/эпик/баг.
    ///
    /// <br/><br/>
    /// <b>Типы задач (itemTypeId):</b>
    /// <ul>
    /// <li>1 – Task (ParentId должен быть <c>null</c> или ссылаться на другую задачу или эпик)</li>
    /// <li>2 – Epic (ParentId должен быть <c>null</c> ВСЕГДА)</li>
    /// <li>3 – Bug</li>
    /// </ul>
    ///
    /// Также необходимо указать <c>boardId</c> и <c>statusId</c> — <c>statusId</c> передается отдельно, внутри модели <c>Item</c> указывать его не нужно.
    /// </remarks>
    /// <param name="item">Модель создания задачи</param>
    [SwaggerOperation("Добавление новой задачи/эпика/бага")]
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

    /// <summary>
    /// Получение задач текущего пользователя.
    /// </summary>
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

    /// <summary>
    /// Получение задач пользователя в проекте.
    /// </summary>
    /// <param name="projectId">ID проекта</param>
    /// <param name="userId">ID пользователя</param>
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

    /// <summary>
    /// Привязать пользователя к задаче.
    /// </summary>
    /// <param name="newUserId">ID пользователя</param>
    /// <param name="itemId">ID задачи</param>
    [HttpPost("add-user-to-item/{itemId}")]
    public async Task<IActionResult> AddUserInItem([FromBody] int newUserId, int itemId)
    {
        try
        {
            await itemManager.AddUserToItemAsync(newUserId, itemId);
            return Ok($"Пользователь {newUserId} присоединен к задаче {itemId}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Изменение типа задачи.
    /// </summary>
    /// /// <remarks>
    /// 
    /// <br/><br/>
    /// <b>Типы задач (itemTypeId):</b>
    /// <ul>
    /// <li>1 – Task (ParentId должен быть <c>null</c> или ссылаться на другую задачу или эпик)</li>
    /// <li>2 – Epic (ParentId должен быть <c>null</c> ВСЕГДА)</li>
    /// <li>3 – Bug</li>
    /// </ul>
    ///
    /// </remarks>
    /// <param name="itemTypeId">ID нового типа</param>
    /// <param name="itemId">ID задачи</param>
    [SwaggerOperation("Изменение типа задачи")]
    [HttpPost("change-itemType/{itemId}")]
    public async Task<IActionResult> ChangeItemType([FromBody] int itemTypeId, int itemId, CancellationToken cancellationToken)
    {
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

    /// <summary>
    /// Изменение параметров задачи.
    /// </summary>
    /// <remarks>
    /// Заменяет все параметры задачи на новые, кроме ID.
    /// </remarks>
    /// <param name="itemModel">Модель задачи с изменёнными параметрами</param>
    [HttpPost("change-params")]
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

    /// <summary>
    /// Добавление комментария к задаче.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <param name="commentModel">Модель комментария</param>
    [HttpPost("comment")]
    public async Task<IActionResult> AddComment([FromBody] NewCommentModel commentModel, CancellationToken cancellationToken)
    {
        try
        {
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Получение всех задач.
    /// </summary>
    [HttpGet("get")]
    [ProducesResponseType<IEnumerable<ItemModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetItemsAsync()
    {
        var items = await itemManager.GetAllItemsAsync();
        return Ok(items);
    }

    /// <summary>
    /// Удаление задачи по ID.
    /// </summary>
    /// <param name="id">ID задачи</param>
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await itemManager.Delete(id);
        return Ok(id);
    }

    /// <summary>
    /// Получение задачи по ID.
    /// </summary>
    /// <param name="id">ID задачи</param>
    [HttpGet("get/{id}")]
    [ProducesResponseType<ItemModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetItemByIdAsync(int id)
    {
        var item = await itemManager.GetByIdAsync(id);
        return Ok(item);
    }

    /// <summary>
    /// Получение задач по ID доски.
    /// </summary>
    /// <param name="boardId">ID доски</param>
    [HttpGet("board/{boardId}")]
    public async Task<IActionResult> GetItemsByBoardIdAsync(int boardId)
    {
        var items = await itemManager.GetByBoardIdAsync(boardId);
        return Ok(items);
    }

    /// <summary>
    /// Получение задачи по названию.
    /// </summary>
    /// <param name="title">Название задачи</param>
    [HttpGet("{title}")]
    public async Task<IActionResult> GetItemByName(string title)
    {
        var item = await itemManager.GetByTitle(title);
        return Ok(item);
    }

    /// <summary>
    /// Получение архивированных задач проекта.
    /// </summary>
    /// <param name="projectId">ID проекта</param>
    [HttpGet("archieved-items/project/{projectId}")]
    [ProducesResponseType<ItemModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetArchievedItemsInProject(int projectId)
    {
        try
        {
            return Ok(await itemManager.GetArchievedItemsInProject(projectId));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Получение архивированных задач доски.
    /// </summary>
    /// <param name="boardId">ID доски</param>
    [HttpGet("archieved-items/board/{boardId}")]
    [ProducesResponseType<ItemModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetArchievedItemsInBoard(int boardId)
    {
        try
        {
            return Ok(await itemManager.GetArchievedItemsInBoard(boardId));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


}