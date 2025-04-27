using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using SharedLibrary.Auth;
using SharedLibrary.Dapper;
using SharedLibrary.Dapper.DapperRepositories;
using SharedLibrary.ProjectModels;

namespace ProjectService.BusinessLayer.Implementations
{
    public class ProjectManager : IProjectManager
    {

        private readonly IProjectRepository _projectRepository;
        private readonly IAuth _auth;
        public ProjectManager(IProjectRepository projectRepository, IAuth auth)
        {
            _projectRepository = projectRepository;
            _auth = auth;
        }

        public async Task<int> Create(ProjectModel project)
        {
            var currentUserId = _auth.GetCurrentUserId();

            if (currentUserId == -1)
                throw new NotAuthorizedException();

            return await _projectRepository.Create(project, (int)currentUserId);
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectModel?> GetById(int id)
        {
            var currentUserId = _auth.GetCurrentUserId();

            if (await _projectRepository.IsUserCanView((int)currentUserId!, id))
                return await _projectRepository.GetById(id);

            throw new NotAuthorizedException("У пользователя нет доступа к проекту");
        }

        public async Task<bool> IsUserInProject(int userId, int projectId)
        {
            var user = await UserRepository.GetUser(userId);
            var project = await GetById(projectId);

            if (user is null || project is null)
                throw new ProjectNotFoundException("User or project not found");

            return await _projectRepository.IsUserInProject(userId, projectId);
        }

        public async Task<bool> IsUserAdmin(int userId, int projectId)
        {
            var user = await UserRepository.GetUser(userId);
            var project = await GetById(projectId);

            if (user is null || project is null)
                throw new ProjectNotFoundException("User or project not found");

            return await _projectRepository.IsUserAdmin(userId, projectId);
        }

        public async Task<bool> IsUserCanView(int userId, int projectId)
        {
            return await _projectRepository.IsUserCanView(userId, projectId);
        }


        //Подумать над логикой
        public async Task<int> AddUserInProject(int userId, int projectId)
        {
            var user = await UserRepository.GetUser(userId);

            var project = _projectRepository.GetById(projectId);

            if (user is null || project is null)
                throw new ProjectNotFoundException("User or project not found");

            return await _projectRepository.AddUserInProject(userId, projectId);
        }

        public Task<int> Update(ProjectModel project)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectModel?> GetByBoardId(int id)
        {
            return await _projectRepository.GetByBoardId(id);
        }
    }
}
