using SharedLibrary.ProjectModels;

namespace ProjectService.BusinessLayer.Abstractions
{
    public interface IProjectLinkManager
    {
        Task<string> Create(int projectId);
        Task<ProjectLinkModel?> Get(int id);
        Task<ProjectLinkModel?> GetByLink(string link);
    }
}
