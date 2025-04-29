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

        var boards = await GetByProjectIdAsync(board.ProjectId);

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

    public async Task ChangeBoardOrderAsync(int boardId, int newOrder)
    {
        var userId = auth.GetCurrentUserId();
        var boardToMove = await boardRepository.GetByIdAsync(boardId);

        if (await userProjectManager.IsUserAdminAsync((int)userId!, boardToMove.ProjectId))
        {
            var boards = await boardRepository.GetByProjectIdAsync(boardToMove.ProjectId);
            ;

            var oldOrder = boardToMove.Status.Order;

            if (newOrder == oldOrder)
                return;

            if (newOrder > oldOrder)
                // Сдвигаем вверх доски между старым и новым порядком
                foreach (var board in boards.Where(b => b.Status.Order > oldOrder && b.Status.Order <= newOrder))
                    board.Status.Order--;
            else
                // Сдвигаем вниз доски между новым и старым порядком
                foreach (var board in boards.Where(b => b.Status.Order >= newOrder && b.Status.Order < oldOrder))
                    board.Status.Order++;

            // Ставим новый порядок для нашей доски
            boardToMove.Status.Order = newOrder;

            await boardRepository.UpdateRangeAsync(boards.ToList());
            await boardRepository.UpdateAsync(boardToMove);

            return;
        }

        throw new ProjectNotFoundException("Проект не найден либо текущий пользователь не имеет полномочий");
    }
}