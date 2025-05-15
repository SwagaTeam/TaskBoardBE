namespace ProjectService.DataLayer.Repositories.Abstractions;

public interface IItemBoardsRepository
{
    Task Create(ItemBoardEntity itemBoard);
}