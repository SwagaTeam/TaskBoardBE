using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Mapper;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;
using System.Linq;
using System.Xml.Linq;
using static Dapper.SqlMapper;

namespace ProjectService.DataLayer.Repositories.Implementations
{
    public class BoardRepository : IBoardRepository
    {
        private readonly ProjectDbContext _projectDbContext;
        public BoardRepository(ProjectDbContext context)
        {
            _projectDbContext = context;
        }
        public async Task Create(BoardEntity board)
        {
            await _projectDbContext.Boards.AddAsync(board);

            await _projectDbContext.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var board = await _projectDbContext.Boards.FindAsync(id);

            if (board != null)
            {
                _projectDbContext.Boards.Remove(board);
                await _projectDbContext.SaveChangesAsync();
            }
        }

        public async Task<BoardEntity?> GetById(int id)
        {
            var board = await _projectDbContext.Boards
                                .Include(x=>x.Status)
                                .FirstOrDefaultAsync(x=>x.Id == id);

            if (board == null)
                return null;

            return board;
        }

        public async Task<BoardEntity?> GetByName(string name)
        {
            var board = await _projectDbContext.Boards
                                .FirstOrDefaultAsync(x => x.Name == name);
            

            return board;
        }

        public async Task<IQueryable<BoardEntity>> GetByProjectId(int projectId)
        {
            var boards = _projectDbContext.Boards
                                .Include(x=>x.Status)
                                .Where(x => x.ProjectId == projectId);

            return boards;
        }

        public async Task Update(BoardEntity board)
        {
            _projectDbContext.Update(board);
            await _projectDbContext.SaveChangesAsync();
        }

        public async Task UpdateRange(ICollection<BoardEntity> boards)
        {
            foreach (var board in boards)
            {
                _projectDbContext.Update(board);
            }
            await _projectDbContext.SaveChangesAsync();
        }
    }
}
