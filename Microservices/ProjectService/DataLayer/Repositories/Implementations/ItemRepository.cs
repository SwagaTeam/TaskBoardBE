using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Implementations;

public class ItemRepository(ProjectDbContext context) : IItemRepository
{
    public async Task<ItemEntity> GetByIdAsync(int id)
    {
        var item = await context.Items
            .Include(x => x.ItemType)
            .FirstOrDefaultAsync(x => x.Id == id);
        return item;
    }

    public async Task CreateAsync(ItemEntity item)
    {
        await context.Items.AddAsync(item);

        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ItemEntity item)
    {
        var existing = await context.Items.FindAsync(item.Id);

        if (existing is null) throw new ItemNotFoundException();
        existing.Id = item.Id;
        existing.ItemTypeId = item.ItemTypeId;
        existing.Description = item.Description;
        existing.ParentId = item.ParentId;
        existing.Priority = item.Priority;
        existing.ProjectId = item.ProjectId;
        existing.StatusId = item.StatusId;
        existing.ProjectItemNumber = item.ProjectItemNumber;
        existing.ExpectedEndDate = item.ExpectedEndDate;
        existing.UpdatedAt = item.UpdatedAt;
        existing.StartDate = item.StartDate;
        existing.IsArchived = item.IsArchived;
        existing.BusinessId = item.BusinessId;
        existing.Title = item.Title;
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
        return await context.Items.FirstOrDefaultAsync(item => item.Title == title);
    }

    public async Task<ICollection<ItemEntity>> GetItemsByUserIdAsync(int userId, int projectId)
    {
        var items = await context.Items
            .Include(i => i.Status)
            .Include(i => i.UserItems)
            .Where(i => i.UserItems.Any(x => x.UserId == userId && i.ProjectId == projectId))
            .ToListAsync();

        return items;
    }

    public async Task<ICollection<ItemEntity>> GetItemsByBoardIdAsync(int boardId)
    {
        var items = await context.Items
            .Include(i => i.Status)
            .Where(i => i.ItemsBoards.Any(ib => ib.BoardId == boardId))
            .ToListAsync();

        return items;
    }

    public async Task AddUserToItemAsync(UserItemEntity userItemEntity)
    {
        await context.UserItems.AddAsync(userItemEntity);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<ItemEntity>> GetCurrentUserItemsAsync(int userId)
    {
        var items = await context.Items
            .Include(i => i.Status)
            .Include(i => i.UserItems)
            .Include(i=>i.ItemsBoards)
            .Where(i => i.UserItems.Any(x => x.UserId == userId))
            .ToListAsync();

        return items;
    }


    public async Task<ICollection<ItemEntity>> GetItemsByProjectIdAsync(int projectId)
    {
        var items = await context.Items
            .Include(i => i.Status)
            .Where(i => i.ProjectId == projectId)
            .ToListAsync();

        return items;
    }
}