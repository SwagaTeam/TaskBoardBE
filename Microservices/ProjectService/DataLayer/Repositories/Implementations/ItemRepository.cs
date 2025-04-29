using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.Entities.ProjectService;
using System.Collections.Specialized;

namespace ProjectService.DataLayer.Repositories.Implementations;

public class ItemRepository(ProjectDbContext context) : IItemRepository
{
    public async Task<ItemEntity> GetByIdAsync(int id)
    {
        var item = await context.Items
            .Include(x=>x.ItemType)
            .FirstOrDefaultAsync(x=>x.Id==id);
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

    public async Task<ICollection<ItemEntity>> GetByBoardIdAsync(int boardId)
    {
        var items = await context.Items
            .Include(i => i.Status)
            .Where(i => i.ItemsBoards.Any(ib => ib.BoardId == boardId))
            .ToListAsync();

        return items;
    }
}