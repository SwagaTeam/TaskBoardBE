﻿using SharedLibrary.Models;
using SharedLibrary.ProjectModels;

namespace ProjectService.BusinessLayer.Abstractions
{
    public interface IBoardManager
    {
        Task<BoardModel?> GetByIdAsync(int id);
        Task<ICollection<BoardModel>> GetByUserIdAsync(int userId);
        Task<ICollection<BoardModel>> GetCurrentBoardsAsync();
        Task<ICollection<BoardModel>> GetByProjectIdAsync(int projectId);
        Task<int> CreateAsync(BoardModel board);
        Task<int> UpdateAsync(BoardModel board);
        Task<int> DeleteAsync(int id);
    }
}
