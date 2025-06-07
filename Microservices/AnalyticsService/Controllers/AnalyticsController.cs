using AnalyticsService.BusinessLayer.Abstractions;
using AnalyticsService.BusinessLayer.Implementations;
using AnalyticsService.Models;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models.AnalyticModels;

namespace AnalyticsService.Controllers
{
    [ApiController]
    [Route("analytics")]
    public class AnalyticsController(ITaskManager taskManager, IProjectManager projectManager) : ControllerBase
    {
        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] SharedLibrary.Models.AnalyticModels.TaskHistoryModel entity)
        {
            try
            {
                var id = await taskManager.CreateAsync(entity);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpGet("history/{projectId}")]
        public async Task<IActionResult> CreateAsync(int projectId)
        {
            try
            {
                var history = await projectManager.GetProjectHistory(projectId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        /// <summary>
        /// Получение данных для диаграммы сгорания задач по проекту
        /// </summary>
        /// <remarks>
        /// <b>Уровни приоритета (priority):</b>
        ///     <ul>
        ///         <li>0 – Очень низкий</li>
        ///         <li>1 – Низкий</li>
        ///         <li>2 – Средний</li>
        ///         <li>3 – Высокий</li>
        ///         <li>4 – Критический</li>
        ///     </ul>
        /// </remarks>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("burndown")]
        public async Task<IActionResult> GetBurnDownChart([FromQuery]BurnDownChartRequest request)
        {
            try
            {
                var result = await projectManager.GetBurndown(request);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Получение завершенных задач за промежуток времени
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("get-completed-tasks-between-datees/{projectId}")]
        public async Task<IActionResult> GetCompletedTasksBetweenDates(int projectId, [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            try
            {
                var items = await taskManager.GetCompletedTaskBetween(projectId, startDate, endDate);
                return Ok(items);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Получение количества завершенных задач за промежуток времени
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("get-completed-task-count-between-dates/{projectId}")]
        public async Task<IActionResult> GetCompletedTaskCountBetweenDates(int projectId,
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                var cnt = await taskManager.GetCompletedTaskCountBetween(projectId, startDate, endDate);
                return Ok(cnt);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        /// <summary>
        /// Получение среднего времени нахождения задачи в определенном статусе
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("get-avg-time-in-status/{taskId}")]
        public async Task<IActionResult> GetAvgTimeInStatus(int taskId, [FromQuery] string status)
        {
            try
            {
                var result = await taskManager.GetAverageTimeInStatusAsync(taskId, status);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        
        /// <summary>
        /// Получение среднего времени нахожления таски вне определенного статусе, в основном предназначено
        /// для использования подсчета среднего времени до завершения задачи.
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("get-avg-time-out-status/{taskId}")]
        public async Task<IActionResult> GetAvgTimeOutStatus(int taskId, [FromQuery] string status)
        {
            try
            {
                var result = await taskManager.GetTotalTimeOutsideStatusAsync(taskId, status);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Получение среднего времени нахождения задачи в каждом статусе
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        // РАБОТАЕТ ТОК ОН!!!
        [HttpGet("get-avg-time-in-statuses/{taskId}")]
        public async Task<IActionResult> GetAvgTimeInStatuses(int taskId)
        {
            try
            {
                var result = await taskManager.GetAverageTimeInStatusesAsync(taskId);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
