using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.DataLayer.Repositories.Implementations;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using SharedLibrary.Auth;
using SharedLibrary.Constants;
using SharedLibrary.Dapper.DapperRepositories;
using SharedLibrary.ProjectModels;

namespace ProjectService.BusinessLayer.Implementations;

public class ProjectManager(
    IProjectRepository projectRepository,
    IUserProjectManager userProjectManager,
    IAuth auth) : IProjectManager
{
    public async Task<int> CreateAsync(ProjectModel project)
    {
        var projectEntity = ProjectMapper.ToEntity(project);
        projectEntity.CreatedAt = DateTime.UtcNow;
        var currentUserId = auth.GetCurrentUserId();

        if (currentUserId == -1)
            throw new NotAuthorizedException();

        await projectRepository.Create(projectEntity);

        var userProject = new UserProjectModel
        {
            ProjectId = projectEntity.Id,
            UserId = (int)currentUserId,
            RoleId = DefaultRoles.CREATOR,
            Privilege = Privilege.ADMIN
        };

        await userProjectManager.CreateAsync(userProject);
        return projectEntity.Id;
    }

    public async Task DeleteAsync(int id)
    {
        var currentUserId = auth.GetCurrentUserId();

        if (await IsUserAdminAsync((int)currentUserId!, id))
            await projectRepository.Delete(id);
        else
            throw new NotAuthorizedException("У пользователя нет доступа к проекту");
    }

    public async Task<ProjectModel?> GetByIdAsync(int id)
    {
        var currentUserId = auth.GetCurrentUserId();

        if (await userProjectManager.IsUserCanViewAsync((int)currentUserId!, id))
        {
            var project = await projectRepository.GetByIdAsync(id);
            if (project == null)
                throw new ProjectNotFoundException();
            return ProjectMapper.ToModel(project);
        }

        throw new NotAuthorizedException("У пользователя нет доступа к проекту");
    }

    public async Task<bool> IsUserInProjectAsync(int userId, int projectId)
    {
        var user = await UserRepository.GetUser(userId);
        var project = await GetByIdAsync(projectId);

        if (user is null || project is null)
            throw new ProjectNotFoundException("Пользователь или проект не найден");

        return await userProjectManager.IsUserInProjectAsync(userId, projectId);
    }

    public async Task<bool> IsUserViewerAsync(int userId, int projectId)
    {
        var user = await UserRepository.GetUser(userId);
        var project = await GetByIdAsync(projectId);
        if (user is null || project is null)
            throw new ProjectNotFoundException("Пользователь или проект не найден");
        return await userProjectManager.IsUserViewerAsync(userId, projectId);
    }

    public async Task<bool> IsUserAdminAsync(int userId, int projectId)
    {
        var user = await UserRepository.GetUser(userId);
        var project = await GetByIdAsync(projectId);

        if (user is null || project is null)
            throw new ProjectNotFoundException("Пользователь или проект не найден");

        return await userProjectManager.IsUserAdminAsync(userId, projectId);
    }

    public async Task<bool> IsUserCanViewAsync(int userId, int projectId)
    {
        return await userProjectManager.IsUserCanViewAsync(userId, projectId);
    }


    //Подумать над логикой
    public async Task<int> AddUserInProjectAsync(int userId, int projectId)
    {
        var user = await UserRepository.GetUser(userId);

        var project = await projectRepository.GetByIdAsync(projectId);

        if (user is null || project is null)
            throw new ProjectNotFoundException("User or project not found");

        var entity = new UserProjectModel
        {
            ProjectId = projectId,
            UserId = userId,
            Privilege = Privilege.MEMBER
        };
        await userProjectManager.CreateAsync(entity);
        return entity.Id;
    }

    public async Task UpdateAsync(ProjectModel project)
    {
        await projectRepository.Update(ProjectMapper.ToEntity(project));
    }

    public async Task<ProjectModel?> GetByBoardIdAsync(int id)
    {
        var board = await projectRepository.GetByBoardIdAsync(id);
        return ProjectMapper.ToModel(board);
    }

    public async Task<int> SetUserRoleAsync(int userId, int projectId, RoleModel role)
    {
        var roleEntity = RoleMapper.ToEntity(role);
        var currentUserId = auth.GetCurrentUserId();

        var project = await projectRepository.GetByIdAsync(projectId);

        if (project is null)
            throw new ProjectNotFoundException();
        bool isCurrentUserAdminAndUserInProject = false;

        if (await userProjectManager.IsUserAdminAsync((int)currentUserId, project.Id)
            && project.UserProjects.Any(x => x.UserId == userId && x.ProjectId == projectId))
            isCurrentUserAdminAndUserInProject = true;

        if (isCurrentUserAdminAndUserInProject)
            return await projectRepository.SetUserRoleAsync(userId, projectId, roleEntity);

        throw new NotAuthorizedException("Пользователь не админ проекта");
    }

}