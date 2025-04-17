using SharedLibrary.ProjectModels;

namespace ProjectService.DataLayer.Repositories.Abstractions
{
    public interface IProjectLinkRepository
    {
        Task<int> Create(ProjectLinkModel projectLink);
        Task<ProjectLinkModel?> Get(int id);
        Task<ProjectLinkModel?> GetByLink(string link);
    }
}
