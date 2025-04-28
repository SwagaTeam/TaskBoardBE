using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Implementations;

public class ItemRepository(ProjectDbContext context) : IItemRepository
{
    public async Task<ItemEntity> GetByIdAsync(int id)
    {
        var item = await context.Items.FindAsync(id);
        return item;
    }

    public async Task CreateAsync(ItemEntity item)
    {
        await context.Items.AddAsync(item);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ItemEntity item)
    {
        context.Items.Update(item);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var item = await GetByIdAsync(id);
        context.Items.Remove(item);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<ItemEntity>> GetItemsAsync()
    {
        return await context.Items.ToListAsync();
    }

    public async Task<ItemEntity> GetByNameAsync(string title)
    {
        return await context.Items.FirstOrDefaultAsync(item=>item.Title == title);
    }
}