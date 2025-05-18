using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Abstractions;

public interface IContributorsRepository
{
    public Task<ICollection<UserProjectEntity>> GetByProjectId(int projectId);
}