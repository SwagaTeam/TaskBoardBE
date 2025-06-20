﻿using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Abstractions;

public interface IProjectRepository
{
    Task<ProjectEntity?> GetByIdAsync(int id);
    Task<ProjectEntity?> GetByBoardIdAsync(int id);
    Task<int> SetUserRoleAsync(int userId, int projectId, RoleEntity role);
    Task Create(ProjectEntity projectEntity);
    Task Update(ProjectEntity projectEntity);
    Task Delete(int id);
    IQueryable<ProjectEntity?> GetByUserId(int? currentUserId);
}