using AnalyticsService.BusinessLayer.Abstractions;
using AnalyticsService.BusinessLayer.Implementations;
using AnalyticsService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Models.AnalyticModels;

namespace AnalyticsService.Controllers
{
    [ApiController]
    [Route("analytics")]
    public class AnalyticsController(ITaskManager taskManager, IProjectManager projectManager) : ControllerBase
    {
        /// <summary>
        /// Создание записи "истории"
        /// </summary> 
        /// <param name="entity"></param>
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

        /// <summary>
        /// Получение истории проекта
        /// </summary> 
        /// <param name="projectId"></param>
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
        /// Получение агрегированных данных для построения кастомной диаграммы по задачам проекта.
        /// </summary>
        /// <remarks>
        /// <b>Параметры запроса:</b>
        /// <ul>
        ///     <li><b>XAxis</b> – поле группировки данных по оси X (обязательный параметр):</li>
        ///     <ul>
        ///         <li><b>status</b> – группировка по статусам задач (например, "To Do", "In Progress").</li>
        ///         <li><b>user</b> – группировка по исполнителям задач (по полю Contributors).</li>
        ///         <li><b>date</b> – группировка по дате начала задачи (StartDate, формат yyyy-MM-dd).</li>
        ///     </ul>
        ///     <li><b>YAxis</b> – метрика по оси Y (необязательный, по умолчанию "count"):</li>
        ///     <ul>
        ///         <li><b>count</b> – количество задач в каждой группе.</li>
        ///         <li><b>sum-priority</b> – сумма приоритетов задач в группе (используется поле Priority).</li>
        ///         <li><b>avg-duration</b> – средняя длительность задач в днях (ExpectedEndDate - StartDate).</li>
        ///     </ul>
        ///     <li><b>Start</b> и <b>End</b> – временной диапазон для выборки задач по дате начала (StartDate).</li>
        /// </ul>
        /// </remarks>
        /// <param name="query">Параметры запроса для построения диаграммы</param>
        /// <returns>Список агрегированных точек данных для отображения диаграммы</returns>
        [HttpGet("custom-chart")]
        public async Task<IActionResult> GetCustomChart([FromQuery] ChartQueryModel query)
        {
            try
            {
                var data = await projectManager.GetCustomChart(query);
                return Ok(data);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получение данных для Roadmap представления
        /// </summary>
        /// <remarks>
        /// Отображает задачи проекта по датам начала и окончания в формате, пригодном для календарного отображения.
        /// </remarks>
        /// <param name="projectId">ID проекта</param>
        /// <returns>Список задач с временными рамками</returns>
        [HttpGet("roadmap/{projectId}")]
        public async Task<IActionResult> GetRoadmap(int projectId)
        {
            try
            {
                var roadmapData = await projectManager.GetRoadmapDataAsync(projectId);
                return Ok(roadmapData);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при получении данных Roadmap: {ex.Message}");
            }
        }

        [HttpGet("heatmap")]
        public async Task<IActionResult> GetHeatmap([FromQuery] HeatmapQueryModel query)
        {
            try
            {
                var data = await projectManager.GetHeatmapData(query);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Получение данных для диаграммы Ганта
        /// </summary>
        /// <remarks>
        /// Возвращает список задач с временными рамками (StartDate, ExpectedEndDate) для визуализации в диаграмме Ганта.
        /// </remarks>
        /// <param name="projectId">ID проекта</param>
        /// <returns>Список задач для диаграммы Ганта</returns>
        [HttpGet("gantt-chart/{projectId}")]
        public async Task<IActionResult> GetGanttChart(int projectId)
        {
            try
            {
                var ganttData = await projectManager.GetGanttChartDataAsync(projectId);
                return Ok(ganttData);
            }
            catch (Exception ex)
            {
                return BadRequest($"Ошибка при получении диаграммы Ганта: {ex.Message}");
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
        ///         <li>Любое другое число - любой приоритет</li>
        ///     </ul>
        /// </remarks>
        /// <param name="request"></param>
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
