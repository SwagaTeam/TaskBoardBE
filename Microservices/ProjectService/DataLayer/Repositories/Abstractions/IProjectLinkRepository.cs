using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Abstractions;

public interface IProjectLinkRepository
{
    Task CreateAsync(ProjectLinkEntity projectLink);
    Task<ProjectLinkEntity?> GetByIdAsync(int id);
    Task<ProjectLinkEntity?> GetByLinkAsync(string link);
}