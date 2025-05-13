using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.Entities.ProjectService;
using System.Collections.Specialized;
using ProjectService.Exceptions;

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
        var existing = await context.Items.FindAsync(item.Id);

        if (existing is not null)
        {
            existing.Id=item.Id;
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
            return;
        }
        
        throw new ItemNotFoundException();
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

    public async Task AddUserToItem(UserItemEntity userItemEntity)
    {
        await context.UserItems.AddAsync(userItemEntity);
        await context.SaveChangesAsync();
    }

    public async Task<ICollection<ItemEntity>> GetItemsByUserId(int userId)
    {
        var items = await context.Items
            .Include(i => i.Status)
            .Include(i => i.UserItems)
            .Where(i => i.UserItems.Any(x=>x.UserId == userId))
            .ToListAsync();

        return items;
    }

    public async Task UpdateStatusAsync(ItemEntity item)
    {
        await context.SaveChangesAsync();
    }
}