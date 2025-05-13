using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("board")]
    public class BoardController : ControllerBase
    {
        private readonly IBoardManager _boardManager;
        private readonly IStatusManager _statusManager;

        public BoardController(IBoardManager boardManager)
        {
            _boardManager = boardManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BoardModel boardModel)
        {
            try
            {
                return Ok(await _boardManager.CreateAsync(boardModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update([FromBody] BoardModel boardModel)
        {
            try
            {
                return Ok(await _boardManager.UpdateAsync(boardModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Изменение порядка колонки
        /// </summary>
        /// <remarks>
        /// Этот метод меняет порядок статуса (колонки) в доске, двигая все остальные статусы в соответствующую сторону.
        /// </remarks>
        /// <param name="updateOrderModel">ID изменяемого статуса, ID доски в которой находится статус, новый порядок (отсчёт с 0).</param>
        [SwaggerOperation("Изменение порядка колонки")]
        [HttpPatch("change-status-order")]
        public async Task<IActionResult> ChangeStatusOrder([FromBody] UpdateOrderModel updateOrderModel)
        {
            try
            {
                await _statusManager.ChangeStatusOrderAsync(updateOrderModel);
                return Ok("Порядок изменён");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Добавление новой колонки
        /// </summary>
        /// <remarks>
        /// Этот метод добавляет новую колонку в конец доски, указывать Order не нужно.
        /// </remarks>
        /// <param name="statusModel">Модель колонки, указывать Order не нужно.</param>
        [SwaggerOperation("Добавление новой колонки")]
        [HttpPost("create-status")]
        public async Task<IActionResult> AddNewStatus([FromBody] StatusModel statusModel)
        {
            try
            {
                await _statusManager.CreateAsync(statusModel);
                return Ok("Порядок изменён");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return Ok(await _boardManager.DeleteAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                return Ok(await _boardManager.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("project/{id}")]
        public async Task<IActionResult> GetByProjectId(int id)
        {
            try
            {
                return Ok(await _boardManager.GetByProjectIdAsync(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }    
    }
}
