using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Implementations;

public class BoardRepository(ProjectDbContext context) : IBoardRepository
{
    public async Task CreateAsync(BoardEntity board)
    {
        await context.Boards.AddAsync(board);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var board = await GetByIdAsync(id);

        if (board is null) throw new NullReferenceException("Доска не найдена");
        
        context.Boards.Remove(board);
        await context.SaveChangesAsync();
    }

    public async Task<BoardEntity?> GetByIdAsync(int id)
    {
        var board = await context.Boards
            .Include(x => x.Statuses)
            .ThenInclude(x=>x.Items) //?
            .FirstOrDefaultAsync(x => x.Id == id);

        return board;
    }

    public async Task<BoardEntity?> GetByNameAsync(string name)
    {
        var board = await context.Boards
            .FirstOrDefaultAsync(x => x.Name == name);


        return board;
    }

    public async Task<IQueryable<BoardEntity>> GetByProjectIdAsync(int projectId)
    {
        var boards = context.Boards
            .Include(x => x.Statuses)
            .Where(x => x.ProjectId == projectId);

        return boards;
    }

    public async Task UpdateAsync(BoardEntity board)
    {
        context.Boards.Update(board);
        await context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(ICollection<BoardEntity> boards)
    {
        context.Boards.UpdateRange(boards);
        await context.SaveChangesAsync();
    }
}