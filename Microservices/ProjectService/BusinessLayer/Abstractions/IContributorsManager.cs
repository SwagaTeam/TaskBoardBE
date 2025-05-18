using SharedLibrary.Entities.ProjectService;

namespace ProjectService.BusinessLayer.Abstractions;

public interface IContributorsManager
{
    Task<ICollection<UserProjectEntity>> GetUserByProjectIdAsync(int projectId);
}