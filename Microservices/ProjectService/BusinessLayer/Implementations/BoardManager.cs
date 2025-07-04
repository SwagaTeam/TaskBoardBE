﻿using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using SharedLibrary.Auth;
using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models;

namespace ProjectService.BusinessLayer.Implementations;

public class BoardManager(IBoardRepository boardRepository, IAuth auth, IProjectManager projectManager, 
    IValidateBoardManager validatorManager, IUserRepository userRepository) : IBoardManager
{
    public async Task<int> CreateAsync(BoardModel board)
    {
        await validatorManager.ValidateUserAdminAsync(board.ProjectId);

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
        await validatorManager.ValidateUserCanViewAsync(projectId);
        await projectManager.GetByIdAsync(projectId); //КОСТЫЛЬ, НЕ УБИРАТЬ!!!!
        var boardsEntities = await boardRepository.GetByProjectIdAsync(projectId);
        var boardModels = await Task.WhenAll(
            boardsEntities.Select(b => BoardMapper.ToModel(b, userRepository))
        );

        return boardModels.ToList();
    }


    public async Task<int> DeleteAsync(int id)
    {
        var projectId = await projectManager.GetByBoardIdAsync(id);
        await validatorManager.ValidateUserAdminAsync(projectId.Id);
        await boardRepository.DeleteAsync(id);
        return id;
    }

    public async Task<BoardModel?> GetByIdAsync(int id)
    {
        var board = await boardRepository.GetByIdAsync(id);
        if (board is null) throw new BoardNotFoundException($"Доски с id {id} не существует");
        var userProject = await projectManager.GetByBoardIdAsync(id);
        await validatorManager.ValidateUserCanViewAsync(userProject.Id);
        return await BoardMapper.ToModel(board, userRepository);
    }


    public async Task<int> UpdateAsync(BoardModel board)
    {
        await validatorManager.ValidateUserAdminAsync(board.ProjectId);
        await boardRepository.UpdateAsync(BoardMapper.ToEntity(board));
        return board.Id;
    }

    public async Task<ICollection<BoardModel>> GetByUserIdAsync(int userId)
    {
        var boards = (await boardRepository.GetByUserIdAsync(userId)).ToList();
        var result = new List<BoardModel>();

        foreach (var board in boards)
        {
            await validatorManager.ValidateUserCanViewAsync(board.ProjectId, userId);
            result.Add(await BoardMapper.ToModel(board, userRepository));
        }

        return result;
    }

    public async Task<ICollection<BoardModel>> GetCurrentBoardsAsync()
    {
        var userId = auth.GetCurrentUserId();
        if (userId is null || userId == -1) throw new NotAuthorizedException();
        var result = await GetByUserIdAsync((int)userId);
        return result;
    }
}