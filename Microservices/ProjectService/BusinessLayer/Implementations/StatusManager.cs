using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using ProjectService.Models;
using SharedLibrary.Auth;
using SharedLibrary.Models;

namespace ProjectService.BusinessLayer.Implementations;

public class StatusManager(IStatusRepository statusRepository, IAuth auth, IProjectRepository projectRepository,
    IValidateStatusManager validatorManager) : IStatusManager
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
        if (statusModel is null) throw new ArgumentNullException("Нельзя создать пустую модель");
        var entity = StatusMapper.ToEntity(statusModel);
        
        var project = await projectRepository.GetByBoardIdAsync(statusModel.BoardId);
        await validatorManager.ValidateUserInProjectAsync(project.Id);

        var statuses = await statusRepository.GetByBoardIdAsync(statusModel.BoardId);

        var lastOrder = statuses.Any() ? statuses.Max(x => x.Order) : 0;

        entity.Order = lastOrder + 1;

        await statusRepository.CreateAsync(entity);

        return statusModel.Id;

    }

    public async Task<int?> UpdateAsync(StatusModel statusModel)
    {
        if (statusModel is null) throw new ArgumentNullException("Нельзя создать пустую модель");
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
        var statusToMove = await statusRepository.GetByIdAsync(updateOrderModel.StatusId);
        var newOrder = updateOrderModel.Order;
        await validatorManager.ValidateUserAdminAsync(statusToMove.Board.ProjectId);
        
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

    }

    public async Task<IEnumerable<StatusModel>> GetByBoardIdAsync(int id)
    {
        var statuses = await statusRepository.GetByBoardIdAsync(id);
        return statuses.Select(StatusMapper.ToModel);
    }
}