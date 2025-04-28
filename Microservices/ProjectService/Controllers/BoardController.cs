using Microsoft.AspNetCore.Mvc;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.Models;

namespace ProjectService.Controllers
{
    [ApiController]
    [Route("board")]
    public class BoardController : ControllerBase
    {
        private readonly IBoardManager _boardManager;

        public BoardController(IBoardManager boardManager)
        {
            _boardManager = boardManager;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] BoardModel boardModel)
        {
            try
            {
                return Ok(await _boardManager.Create(boardModel));
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
                return Ok(await _boardManager.Update(boardModel));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("change-order")]
        public async Task<IActionResult> ChangeOrder([FromBody] UpdateOrderModel updateOrderModel)
        {
            try
            {
                await _boardManager.ChangeBoardOrder(updateOrderModel.BoardId, updateOrderModel.Order);
                return Ok("Порядок изменён");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                return Ok(await _boardManager.Delete(id));
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
                return Ok(await _boardManager.GetById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get/project/{id}")]
        public async Task<IActionResult> GetByProjectId(int id)
        {
            try
            {
                return Ok(await _boardManager.GetByProjectId(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }    
    }
}
