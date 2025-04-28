using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using SharedLibrary.Auth;
using SharedLibrary.Dapper;
using SharedLibrary.Dapper.DapperRepositories;
using SharedLibrary.Entities.ProjectService;
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
            var projectEntity = ProjectMapper.ToEntity(project);

            projectEntity.CreatedAt = DateTime.UtcNow;

            var currentUserId = _auth.GetCurrentUserId();

            if (currentUserId == -1)
                throw new NotAuthorizedException();

            await _projectRepository.Create(projectEntity, (int)currentUserId);

            return projectEntity.Id;
        }

        public async Task Delete(int id)
        {
            var currentUserId = _auth.GetCurrentUserId();

            if(await IsUserAdmin((int)currentUserId!, id))
                await _projectRepository.Delete(id);
            else
                throw new NotAuthorizedException("У пользователя нет доступа к проекту");
        }

        public async Task<ProjectModel?> GetById(int id)
        {
            var currentUserId = _auth.GetCurrentUserId();

            if (await _projectRepository.IsUserCanView((int)currentUserId!, id))
            {
                var project = await _projectRepository.GetById(id);
                if (project == null)
                    throw new ProjectNotFoundException();
                return ProjectMapper.ToModel(project);
            }
                
            throw new NotAuthorizedException("У пользователя нет доступа к проекту");
        }

        public async Task<bool> IsUserInProject(int userId, int projectId)
        {
            var user = await UserRepository.GetUser(userId);
            var project = await GetById(projectId);

            if (user is null || project is null)
                throw new ProjectNotFoundException("Пользователь или проект не найден");

            return await _projectRepository.IsUserInProject(userId, projectId);
        }

        public async Task<bool> IsUserAdmin(int userId, int projectId)
        {
            var user = await UserRepository.GetUser(userId);
            var project = await GetById(projectId);

            if (user is null || project is null)
                throw new ProjectNotFoundException("Пользователь или проект не найден");

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

        public Task Update(ProjectModel project)
        {
            throw new NotImplementedException();
        }

        public async Task<ProjectModel?> GetByBoardId(int id)
        {
            var board = await _projectRepository.GetByBoardId(id);

            return ProjectMapper.ToModel(board);
        }
    }
}
