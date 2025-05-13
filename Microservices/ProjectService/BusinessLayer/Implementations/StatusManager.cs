using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.DataLayer.Repositories.Implementations;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using ProjectService.Models;
using SharedLibrary.Auth;

namespace ProjectService.BusinessLayer.Implementations;

public class StatusManager(
    IStatusRepository statusRepository, 
    IAuth auth, 
    IProjectRepository projectRepository,
    IUserProjectManager userProjectManager) : IStatusManager
{
    public async Task<IEnumerable<StatusModel>> GetAllAsync()
    {
        return (await statusRepository.GetAllAsync())
            .Select(StatusMapper.ToModel);
    }

    public async Task<StatusModel> GetByIdAsync(int id)
    {
        return StatusMapper.ToModel(await statusRepository.GetByIdAsync(id));
    }

    public async Task<int?> CreateAsync(StatusModel statusModel)
    {
        if (statusModel is null) throw new NullReferenceException("Нельзя создать пустую модель");
        var entity = StatusMapper.ToEntity(statusModel);

        var currentUser = auth.GetCurrentUserId();

        var project = await projectRepository.GetByBoardIdAsync((int)statusModel.Id);

        if (project is not null &&
            currentUser != -1   &&
            await userProjectManager.IsUserInProjectAsync((int)currentUser, project.Id))
        {
            int lastOrder;

            var statuses = await statusRepository.GetByBoardIdAsync(statusModel.BoardId);

            if (statuses.Count() > 0)
                lastOrder = statuses.Max(x => x.Order);
            else lastOrder = 0;

            entity.Order = lastOrder;

            await statusRepository.CreateAsync(entity);

            return statusModel.Id;
        }

        throw new BoardNotFoundException();
    }

    public async Task<int?> UpdateAsync(StatusModel statusModel)
    {
        if (statusModel is null) throw new NullReferenceException("Нельзя создать пустую модель");
        var entity = StatusMapper.ToEntity(statusModel);

        await statusRepository.UpdateAsync(entity);
        return statusModel.Id;
    }

    public async Task DeleteAsync(int id)
    {
        await statusRepository.DeleteAsync(id);
    }

    public async Task ChangeStatusOrderAsync(UpdateOrderModel updateOrderModel)
    {
        var userId = auth.GetCurrentUserId();
        var statusToMove = await statusRepository.GetByIdAsync(updateOrderModel.StatusId);
        var newOrder = updateOrderModel.Order;

        if (await userProjectManager.IsUserAdminAsync((int)userId!, statusToMove.Board.ProjectId))
        {
            var statuses = await statusRepository.GetByBoardIdAsync(statusToMove.BoardId);

            var oldOrder = statusToMove.Order;

            if (newOrder == oldOrder)
                return;

            if (newOrder > oldOrder)
                // Сдвигаем вверх доски между старым и новым порядком
                foreach (var status in statuses.Where(s => s.Order > oldOrder && s.Order <= newOrder))
                    status.Order--;
            else
                // Сдвигаем вниз доски между новым и старым порядком
                foreach (var status in statuses.Where(s => s.Order >= newOrder && s.Order < oldOrder))
                    status.Order++;

            // Ставим новый порядок для нашей доски
            statusToMove.Order = newOrder;

            await statusRepository.UpdateRangeAsync(statuses.ToList());
            await statusRepository.UpdateAsync(statusToMove);

            return;
        }

        throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет полномочий");
    }
}