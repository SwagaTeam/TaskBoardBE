using SharedLibrary.Entities.ProjectService;

namespace Contributors.DataLayer.Repositories.Abstractions;

public interface IContributorsRepository
{
    public Task<ICollection<UserProjectEntity>> GetByProjectId(int projectId);
}