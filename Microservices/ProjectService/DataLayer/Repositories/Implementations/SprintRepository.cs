using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Implementations
{
    public class SprintRepository : ISprintRepository
    {
        private readonly ProjectDbContext dbContext;
        public SprintRepository(ProjectDbContext dbContext)
        {

            this.dbContext = dbContext;
        }

        public async Task AddItem(int sprintId, int itemId)
        {
            var existingSprint = await dbContext.Sprints.FindAsync(sprintId);
            var existingItem = await dbContext.Items.FindAsync(itemId);

            if (existingSprint is not null
                && existingItem is not null)
            {
                existingSprint.Items.Add(existingItem);
                await dbContext.SaveChangesAsync();
            }

            throw new SprintNotFoundException("Спринт или задача не найдены");
        }

        public async Task CreateAsync(SprintEntity sprintEntity)
        {
            await dbContext.Sprints.AddAsync(sprintEntity);
            await dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var existingSprint = await dbContext.Sprints.FindAsync(id);

            if(existingSprint is not null)
            {
                dbContext.Sprints.Remove(existingSprint);
                await dbContext.SaveChangesAsync();
            }

            throw new SprintNotFoundException();
        }

        public async Task<IEnumerable<SprintEntity>> GetAllAsync()
        {
            var sprints = await dbContext.Sprints.ToListAsync();

            return sprints;
        }

        public async Task<IEnumerable<SprintEntity>> GetByBoardId(int boardId)
        {
            var sprints = await dbContext.Sprints
                                .Where(x => x.BoardId == boardId)
                                .ToListAsync();

            return sprints;
        }

        public async Task<SprintEntity> GetByIdAsync(int sprintId)
        {
            var existingSprint = await dbContext.Sprints
                                    .Include(x => x.Board)
                                    .FirstOrDefaultAsync(x => x.Id == sprintId);

            if (existingSprint is not null)
                return existingSprint;

            throw new SprintNotFoundException();
        }

        public async Task UpdateAsync(SprintEntity sprintEntity)
        {
            var existing = await dbContext.Sprints.FindAsync(sprintEntity.Id);

            if (existing is not null)
            {
                existing.Name = sprintEntity.Name;
                existing.StartDate = sprintEntity.StartDate;
                existing.BoardId = sprintEntity.BoardId;
                existing.EndDate = sprintEntity.EndDate;

                await dbContext.SaveChangesAsync();
            }

            throw new SprintNotFoundException();
        }
    }
}
