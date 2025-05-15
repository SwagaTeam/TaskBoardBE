using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using SharedLibrary.Auth;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.BusinessLayer.Implementations;

public class BoardManager(
    IBoardRepository boardRepository,
    IAuth auth,
    IUserProjectManager userProjectManager,
    IProjectManager projectManager)
    : IBoardManager
{
    public async Task<int> CreateAsync(BoardModel board)
    {
        var userId = auth.GetCurrentUserId();

        if (!await userProjectManager.IsUserAdminAsync((int)userId!, board.ProjectId))
            throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет полномочий");

        var boardEntity = BoardMapper.ToEntity(board);

        boardEntity.Statuses =
        [
            new StatusEntity
            {
                Name = "В очереди",
                IsDone = false,
                IsRejected = false,
                Order = 0
            },
            new StatusEntity
            {
                Name = "На исполнении",
                IsDone = false,
                IsRejected = false,
                Order = 1
            },
            new StatusEntity
            {
                Name = "Готово",
                IsDone = true,
                IsRejected = false,
                Order = 2
            },
            new StatusEntity
            {
                Name = "Отклонено",
                IsDone = false,
                IsRejected = true,
                Order = 3
            }
        ];

        board.CreatedAt = DateTime.UtcNow;

        await boardRepository.CreateAsync(boardEntity);

        return boardEntity.Id;
    }

    public async Task<ICollection<BoardModel>> GetByProjectIdAsync(int projectId)
    {
        var userId = auth.GetCurrentUserId();

        if (await userProjectManager.IsUserCanViewAsync((int)userId!, projectId))
        {
            var boardsEntities = await boardRepository.GetByProjectIdAsync(projectId);

            return boardsEntities.Select(BoardMapper.ToModel).ToList();
        }

        throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет полномочий");
    }

    public async Task<int> DeleteAsync(int id)
    {
        var userId = auth.GetCurrentUserId();

        if (await userProjectManager.IsUserAdminAsync((int)userId!, id))
        {
            await boardRepository.DeleteAsync(id);
            return id;
        }

        throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет полномочий");
    }

    public async Task<BoardModel?> GetByIdAsync(int id)
    {
        var userId = auth.GetCurrentUserId();
        var board = await boardRepository.GetByIdAsync(id);
        if (board is null) throw new Exception($"Доски с id {id} не существует");
        var userProject = await projectManager.GetByBoardIdAsync(id);
        if (userProject is not null && await userProjectManager.IsUserCanViewAsync((int)userId, userProject.Id))
            return BoardMapper.ToModel(board);

        throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет доступа к проекту");
    }


    public async Task<int> UpdateAsync(BoardModel board)
    {
        var userId = auth.GetCurrentUserId();

        if (await userProjectManager.IsUserAdminAsync((int)userId!, board.ProjectId))
        {
            await boardRepository.UpdateAsync(BoardMapper.ToEntity(board));
            return board.Id;
        }

        throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет полномочий");
    }
}