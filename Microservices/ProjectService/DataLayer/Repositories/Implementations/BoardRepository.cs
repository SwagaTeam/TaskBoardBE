using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Mapper;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;
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
        public async Task<int> Create(BoardModel board)
        {
            var boardEntity = BoardMapper.ToEntity(board);

            boardEntity.Status = new StatusEntity 
            {
                Name = board.Name,
                IsDone = false, 
                IsRejected = false,
                Order = -1
            };

            boardEntity.CreatedAt = DateTime.UtcNow;

            await _projectDbContext.Boards.AddAsync(boardEntity);

            await _projectDbContext.SaveChangesAsync();

            return boardEntity.Id;
        }

        public async Task<int> Delete(int id)
        {
            var board = await _projectDbContext.Boards.FindAsync(id);

            if (board != null)
            {
                _projectDbContext.Boards.Remove(board);
                await _projectDbContext.SaveChangesAsync();
                return id;
            }

            return -1;
        }

        public async Task<BoardModel?> GetById(int id)
        {
            var board = await _projectDbContext.Boards.FindAsync(id);

            if (board == null)
                return null;

            return BoardMapper.ToModel(board!);
        }

        public async Task<BoardModel?> GetByName(string name)
        {
            var board = await _projectDbContext.Boards.FirstOrDefaultAsync(x => x.Name == name);

            if (board == null)
                return null;

            return BoardMapper.ToModel(board!);
        }

        public Task<int> Update(BoardModel board)
        {
            throw new NotImplementedException();
        }
    }
}
