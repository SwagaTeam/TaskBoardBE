using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using SharedLibrary.Auth;
using SharedLibrary.ProjectModels;

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

            if (await _projectRepository.IsUserAdmin((int)userId!, board.ProjectId))
                await _boardRepository.Create(board);

            throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет полномочий");
        }

        public async Task<int> Delete(int id)
        {
            var userId = _auth.GetCurrentUserId();

            if (await _projectRepository.IsUserAdmin((int)userId!, id))
                await _boardRepository.Delete(id);

            throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет полномочий");
        }

        public async Task<BoardModel?> GetById(int id)
        {
            var userId = _auth.GetCurrentUserId();

            var userProject = await _projectRepository.GetByBoardId(id);

            if (await _projectRepository.IsUserCanView((int)userId, userProject.Id))
                await _boardRepository.GetById(id);

            throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет доступа к проекту");
        }

        public Task<int> Update(BoardModel board)
        {
            throw new NotImplementedException();
        }
    }
}
