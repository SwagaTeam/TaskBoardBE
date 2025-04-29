using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Implementations;

public class ItemTypeRepository(ProjectDbContext context) : IItemTypeRepository
{
    
    public async Task<ItemTypeEntity> GetByIdAsync(int itemTypeId)
    {
        var itemType = await context.ItemTypes.FindAsync(itemTypeId);
        return itemType;
    }

    public async Task<IEnumerable<ItemTypeEntity>> GetAllAsync()
    {
        var itemTypes = await context.ItemTypes.ToListAsync();
        return itemTypes;
    }

    public async Task CreateAsync(ItemTypeEntity itemTypeEntity)
    {
        await context.ItemTypes.AddAsync(itemTypeEntity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var itemType = await GetByIdAsync(id);
        context.ItemTypes.Remove(itemType);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ItemTypeEntity itemTypeEntity)
    {
        context.ItemTypes.Update(itemTypeEntity);
        await context.SaveChangesAsync();
    }
}