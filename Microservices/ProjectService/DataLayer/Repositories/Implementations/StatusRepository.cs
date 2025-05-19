using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.Entities.ProjectService;
using System.Collections.Generic;

namespace ProjectService.DataLayer.Repositories.Implementations;

public class StatusRepository(ProjectDbContext context) : IStatusRepository
{
    public async Task<StatusEntity> GetByIdAsync(int statusId)
    {
        var status = await context.Statuses
                                   .Include(x => x.Board)
                                   .FirstOrDefaultAsync(s => s.Id == statusId);
        return status;
    }

    public async Task<IQueryable<StatusEntity>> GetByBoardIdAsync(int boardId)
    {
        var statuses = context.Statuses
                                   .Where(x=>x.BoardId == boardId);
        return statuses;
    }

    public async Task<IEnumerable<StatusEntity>> GetAllAsync()
    {
        var statuses = await context.Statuses.ToListAsync();
        return statuses;
    }

    public async Task CreateAsync(StatusEntity statusEntity)
    {
        await context.Statuses.AddAsync(statusEntity);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var status = await GetByIdAsync(id);
        context.Statuses.Remove(status);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StatusEntity statusEntity)
    {
        context.Statuses.Update(statusEntity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateRangeAsync(ICollection<StatusEntity> statusEntities)
    {
        context.Statuses.UpdateRange(statusEntities);
        await context.SaveChangesAsync();
    }
}