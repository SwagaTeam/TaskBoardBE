using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using SharedLibrary.Auth;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.ProjectModels;
using System.Reflection.Metadata.Ecma335;

namespace ProjectService.BusinessLayer.Implementations
{
    public class BoardManager : IBoardManager
    {
        private readonly IBoardRepository _boardRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IAuth _auth;
        public BoardManager(IBoardRepository board, IAuth auth, IProjectRepository projectRepository)
        {
            _boardRepository = board;
            _auth = auth;
            _projectRepository = projectRepository;

        }

        public async Task<int> Create(BoardModel board)
        {
            var userId = _auth.GetCurrentUserId();

            if (!await _projectRepository.IsUserAdmin((int)userId!, board.ProjectId))
            {
                throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет полномочий");
            }

            var boards = await GetByProjectId(board.ProjectId);

            int lastOrder;

            if (boards.Count > 0)
                lastOrder = boards.Max(x => x.Status.Order);
            else lastOrder = 0;

            var boardEntity = BoardMapper.ToEntity(board);

            boardEntity.Status = new StatusEntity
            {
                Name = board.Name,
                IsDone = false,
                IsRejected = false,
                Order = lastOrder + 1
            };

            board.CreatedAt = DateTime.UtcNow;
            
            await _boardRepository.Create(boardEntity);

            return boardEntity.Id;
        }

        public async Task<ICollection<BoardModel>> GetByProjectId(int projectId)
        {
            var userId = _auth.GetCurrentUserId();

            if (await _projectRepository.IsUserCanView((int)userId!, projectId))
            {
                var boardsEntities = await _boardRepository.GetByProjectId(projectId);

                return boardsEntities.Select(BoardMapper.ToModel).ToList();
            }

            throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет полномочий");
        }

        public async Task<int> Delete(int id)
        {
            var userId = _auth.GetCurrentUserId();

            if (await _projectRepository.IsUserAdmin((int)userId!, id))
            {
                await _boardRepository.Delete(id);
                return id;
            }

            throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет полномочий");
        }

        public async Task<BoardModel?> GetById(int id)
        {
            var userId = _auth.GetCurrentUserId();

            var userProject = await _projectRepository.GetByBoardId(id);

            if (await _projectRepository.IsUserCanView((int)userId, userProject.Id))
            {
                var board = await _boardRepository.GetById(id);
                return board is null ? null : BoardMapper.ToModel(board);
            }

            throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет доступа к проекту");
        }

        public async Task<int> Update(BoardModel board)
        {
            var userId = _auth.GetCurrentUserId();

            if (await _projectRepository.IsUserAdmin((int)userId!, board.ProjectId))
            {
                await _boardRepository.Update(BoardMapper.ToEntity(board));
                return board.Id;
            }

            throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет полномочий");
        }

        public async Task ChangeBoardOrder(int boardId, int newOrder)
        {
            var userId = _auth.GetCurrentUserId();
            var boardToMove = await _boardRepository.GetById(boardId);

            if (await _projectRepository.IsUserAdmin((int)userId!, boardToMove.ProjectId))
            {
                var boards = await _boardRepository.GetByProjectId(boardToMove.ProjectId); ;

                int oldOrder = boardToMove.Status.Order;

                if (newOrder == oldOrder)
                    return;

                if (newOrder > oldOrder)
                {
                    // Сдвигаем вверх доски между старым и новым порядком
                    foreach (var board in boards.Where(b => b.Status.Order > oldOrder && b.Status.Order <= newOrder))
                    {
                        board.Status.Order--;
                    }
                }
                else
                {
                    // Сдвигаем вниз доски между новым и старым порядком
                    foreach (var board in boards.Where(b => b.Status.Order >= newOrder && b.Status.Order < oldOrder))
                    {
                        board.Status.Order++;
                    }
                }

                // Ставим новый порядок для нашей доски
                boardToMove.Status.Order = newOrder;

                await _boardRepository.UpdateRange(boards.ToList());
                await _boardRepository.Update(boardToMove);

                return;
            }
            throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет полномочий");
        }
    }
}
