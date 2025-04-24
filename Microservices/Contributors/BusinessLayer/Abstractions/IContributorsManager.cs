using SharedLibrary.Entities.ProjectService;

namespace Contributors.BusinessLayer.Abstractions;

public interface IContributorsManager
{
    Task<ICollection<UserProjectEntity>> GetUserByProjectIdAsync(int projectId);
}