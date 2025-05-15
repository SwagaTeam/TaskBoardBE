using ProjectService.DataLayer.Repositories.Abstractions;

namespace ProjectService.DataLayer.Repositories.Implementations;

public class ItemBoardsRepository(ProjectDbContext projectDbContext) : IItemBoardsRepository
{
    public async Task Create(ItemBoardEntity itemBoard)
    {
        await projectDbContext.ItemsBoards.AddAsync(itemBoard);
        await projectDbContext.SaveChangesAsync();
    }
}