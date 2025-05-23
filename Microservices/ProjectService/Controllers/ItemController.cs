using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ProjectService.Controllers;

[ApiController]
[Route("item")]
public class ItemController(IItemManager itemManager) : ControllerBase
{
    /// <summary>
    ///     Добавление новой задачи/эпика/бага.
    /// </summary>
    /// <remarks>
    ///     Этот метод добавляет новую задачу/эпик/баг.
    ///     <br /><br />
    ///     <b>Типы задач (itemTypeId):</b>
    ///     <ul>
    ///         <li>1 – Task (ParentId должен быть <c>null</c> или ссылаться на другую задачу или эпик)</li>
    ///         <li>2 – Epic (ParentId должен быть <c>null</c> ВСЕГДА)</li>
    ///         <li>3 – Bug</li>
    ///     </ul>
    ///     <b>Уровни приоритета (priority):</b>
    ///     <ul>
    ///         <li>0 – Очень низкий</li>
    ///         <li>1 – Низкий</li>
    ///         <li>2 – Средний</li>
    ///         <li>3 – Высокий</li>
    ///         <li>4 – Критический</li>
    ///     </ul>
    ///     Также необходимо указать <c>boardId</c> и <c>statusId</c> — <c>statusId</c> передается внутри модели
    ///     <c>Item</c>, остальные параметры модели указывать не нужно.
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
    ///     Получение задач текущего пользователя.
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
    ///     Получение задач пользователя в проекте.
    /// </summary>
    /// <param name="model">ID пользователя и ID проекта</param>
    [HttpGet("get-user-item")]
    public async Task<IActionResult> GetUserItem([FromBody] GetUserItemModel model)
    {
        try
        {
            var items = await itemManager.GetItemsByUserId(model.UserId, model.ProjectId);
            return Ok(items);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    ///     Привязать пользователя к задаче.
    /// </summary>
    /// <param name="newUserId">ID пользователя</param>
    /// <param name="itemId">ID задачи</param>
    [HttpPost("add-user-to-item/{itemId}")]
    public async Task<IActionResult> AddUserInItem([FromBody] int newUserId, int itemId, CancellationToken token)
    {
        try
        {
            await itemManager.AddUserToItemAsync(newUserId, itemId, token);
            return Ok($"Пользователь {newUserId} присоединен к задаче {itemId}");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    ///     Изменение типа задачи.
    /// </summary>
    /// ///
    /// <remarks>
    ///     <br /><br />
    ///     <b>Типы задач (itemTypeId):</b>
    ///     <ul>
    ///         <li>1 – Task (ParentId должен быть <c>null</c> или ссылаться на другую задачу или эпик)</li>
    ///         <li>2 – Epic (ParentId должен быть <c>null</c> ВСЕГДА)</li>
    ///         <li>3 – Bug</li>
    ///     </ul>
    /// </remarks>
    /// <param name="itemTypeId">ID нового типа</param>
    /// <param name="itemId">ID задачи</param>
    [SwaggerOperation("Изменение типа задачи")]
    [HttpPost("change-itemType/{itemId}")]
    public async Task<IActionResult> ChangeItemType([FromBody] int itemTypeId, int itemId,
        CancellationToken cancellationToken)
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
    ///     Изменение статуса задачи.
    /// </summary>
    /// ///
    /// <remarks>
    /// </remarks>
    /// <param name="statusId">Новый статус</param>
    /// <param name="itemId">ID задачи</param>
    [SwaggerOperation("Изменение статуса задачи")]
    [HttpPost("change-status/{itemId}")]
    public async Task<IActionResult> ChangeStatus([FromBody] int statusId, int itemId,
        CancellationToken cancellationToken)
    {
        try
        {
            var itemModel = await itemManager.GetByIdAsync(itemId);
            itemModel.StatusId = statusId;
            var newItemModel = await itemManager.UpdateAsync(itemModel);
            return Ok(newItemModel);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    ///     Изменение Приоритета задачи.
    /// </summary>
    /// ///
    /// <remarks>
    ///     <br /><br />
    ///     <b>Уровни приоритета (priority):</b>
    ///     <ul>
    ///         <li>0 – Очень низкий</li>
    ///         <li>1 – Низкий</li>
    ///         <li>2 – Средний</li>
    ///         <li>3 – Высокий</li>
    ///         <li>4 – Критический</li>
    ///     </ul>
    /// </remarks>
    /// <param name="priority">Новый приоритет</param>
    /// <param name="itemId">ID задачи</param>
    [SwaggerOperation("Изменение приоритета задачи")]
    [HttpPost("change-priority/{itemId}")]
    public async Task<IActionResult> ChangePriority([FromBody] int priority, int itemId,
        CancellationToken cancellationToken)
    {
        try
        {
            var itemModel = await itemManager.GetByIdAsync(itemId);
            itemModel.Priority = priority;
            var newItemModel = await itemManager.UpdateAsync(itemModel);
            return Ok(newItemModel);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    ///     Изменение параметров задачи.
    /// </summary>
    /// <remarks>
    ///     Заменяет все параметры задачи на новые, кроме ID.
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
    /// Прикреплять файл не обязательно!
    /// </remarks>
    /// <param name="itemId">ID задачи</param>
    /// <param name="text">Текст комментария</param>
    /// <param name="attachment">Прикрепляемый файл</param>

    [HttpPost("comment")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> AddComment(
    [FromForm] int itemId,
    [FromForm] string text,
    IFormFile? attachment)
    {
        try
        {
            var comment = new CommentModel
            {
                ItemId = itemId,
                Text = text,
                CreatedAt = DateTime.UtcNow
            };

            await itemManager.AddCommentToItemAsync(comment, attachment);
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Получение комментариев к задаче.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <param name="itemId">ID задачи</param>
    ///
    [ProducesResponseType<ICollection<CommentModel>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpGet("{itemId}/comments")]
    public async Task<IActionResult> GetComments(int itemId)
    {
        try
        {
            var comments = await itemManager.GetComments(itemId);
            return Ok(comments);
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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetItemsAsync()
    {
        var items = await itemManager.GetAllItemsAsync();
        return Ok(items);
    }

    /// <summary>
    ///     Удаление задачи по ID.
    /// </summary>
    /// <param name="id">ID задачи</param>
    [ProducesResponseType<int>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await itemManager.Delete(id);
        return Ok(id);
    }

    /// <summary>
    ///     Получение задачи по ID.
    /// </summary>
    /// <param name="id">ID задачи</param>
    [HttpGet("{id}")]
    [ProducesResponseType<ItemModel>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetItemByIdAsync(int id)
    {
        var item = await itemManager.GetByIdAsync(id);
        return Ok(item);
    }

    /// <summary>
    ///     Получение задач по ID доски.
    /// </summary>
    /// <param name="boardId">ID доски</param>
    [HttpGet("board/{boardId}")]
    public async Task<IActionResult> GetItemsByBoardIdAsync(int boardId)
    {
        var items = await itemManager.GetByBoardIdAsync(boardId);
        return Ok(items);
    }

    /// <summary>
    ///     Получение архивированных задач проекта.
    /// </summary>
    /// <param name="projectId">ID проекта</param>
    [HttpGet("archieved-items/project/{projectId}")]
    [ProducesResponseType<ItemModel>(StatusCodes.Status200OK)]
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
    ///     Получение архивированных задач доски.
    /// </summary>
    /// <param name="boardId">ID доски</param>
    [HttpGet("archieved-items/board/{boardId}")]
    [ProducesResponseType<ItemModel>(StatusCodes.Status200OK)]
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