using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;
using SharedLibrary.Models;

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

    /// <summary>
    /// Получение спринта по его ID.
    /// </summary>
    /// <param name="id">ID спринта.</param>
    /// <returns>Данные спринта.</returns>
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

    /// <summary>
    /// Получение всех спринтов, связанных с конкретной доской.
    /// </summary>
    /// <param name="boardId">ID доски.</param>
    /// <returns>Список спринтов.</returns>
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

    /// <summary>
    /// Создание нового спринта.
    /// </summary>
    /// <param name="model">Модель данных спринта.</param>
    /// <returns>ID созданного спринта или результат операции.</returns>
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

    /// <summary>
    /// Добавление задачи в спринт.
    /// </summary>
    /// <param name="sprintId">ID спринта.</param>
    /// <param name="itemId">ID задачи для добавления.</param>
    /// <returns>Статус операции.</returns>
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

    /// <summary>
    /// Обновление данных спринта.
    /// </summary>
    /// <param name="model">Обновлённая модель спринта.</param>
    /// <returns>ID обновленной модели.</returns>
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

    /// <summary>
    /// Удаление спринта по ID.
    /// </summary>
    /// <param name="sprintId">ID спринта для удаления.</param>
    /// <returns>ID удалённого спринта.</returns>
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