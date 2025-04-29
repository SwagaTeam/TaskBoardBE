using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Implementations
{
    public class ItemBoardsRepository(ProjectDbContext projectDbContext) : IItemBoardsRepository
    {
        public async Task Create(ItemBoardEntity itemBoard)
        {
            await projectDbContext.ItemsBoards.AddAsync(itemBoard);
            await projectDbContext.SaveChangesAsync();
        }
    }
}
