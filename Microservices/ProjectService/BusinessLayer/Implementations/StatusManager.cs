using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Mapper;

namespace ProjectService.BusinessLayer.Implementations;

public class StatusManager(IStatusRepository statusRepository) : IStatusManager
{
    public async Task<IEnumerable<StatusModel>> GetAll()
    {
        return (await statusRepository.GetAllAsync())
            .Select(StatusMapper.ToModel);
    }

    public async Task<StatusModel> GetById(int id)
    {
        return StatusMapper.ToModel(await statusRepository.GetByIdAsync(id));
    }

    public async Task<int?> Create(StatusModel statusModel)
    {
        var entity = StatusMapper.ToEntity(statusModel);
        if (entity is null) throw new NullReferenceException("Нельзя создать пустую модель");
        
        await statusRepository.CreateAsync(entity);
        return statusModel.Id;
    }

    public async Task<int?> Update(StatusModel statusModel)
    {
        var entity = StatusMapper.ToEntity(statusModel);
        if (entity is null) throw new NullReferenceException("Нельзя создать пустую модель");
        await statusRepository.UpdateAsync(entity);
        return statusModel.Id;
    }

    public async Task Delete(int id)
    {
        await statusRepository.DeleteAsync(id);
    }
}