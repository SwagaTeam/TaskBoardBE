using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Mapper;

namespace ProjectService.BusinessLayer.Implementations;

public class StatusManager(IStatusRepository statusRepository) : IStatusManager
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
        var entity = StatusMapper.ToEntity(statusModel);
        if (entity is null) throw new NullReferenceException("Нельзя создать пустую модель");
        
        await statusRepository.CreateAsync(entity);
        return statusModel.Id;
    }

    public async Task<int?> UpdateAsync(StatusModel statusModel)
    {
        var entity = StatusMapper.ToEntity(statusModel);
        if (entity is null) throw new NullReferenceException("Нельзя создать пустую модель");
        await statusRepository.UpdateAsync(entity);
        return statusModel.Id;
    }

    public async Task DeleteAsync(int id)
    {
        await statusRepository.DeleteAsync(id);
    }
}