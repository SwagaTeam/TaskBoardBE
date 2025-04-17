using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
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
                throw new Exception("Not authorized");

            return await _projectRepository.Create(project, (int)currentUserId);
        }

        public Task<int> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectModel?> GetById(int id)
        {
            return await _projectRepository.GetById(id);
        }

        public async Task<bool> IsUserInProject(int userId, int projectId)
        {
            var user = await UserRepository.GetUser(userId);
            var project = await GetById(projectId);

            if (user is null || project is null)
                throw new Exception("User or project not found");

            return await _projectRepository.IsUserInProject(userId, projectId);
        }

        public async Task<int> AddUserInProject(int userId, int projectId)
        {
            var user = await UserRepository.GetUser(userId);
            var project = await GetById(projectId);

            if (user is null || project is null)
                throw new Exception("User or project not found");

            return await _projectRepository.AddUserInProject(userId, projectId);
        }

        public Task<int> Update(ProjectModel project)
        {
            throw new NotImplementedException();
        }
    }
}
