using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.BusinessLayer.Implementations;

public class ProjectLinkManager(IProjectLinkRepository projectLinkRepository, IProjectRepository projectRepository)
    : IProjectLinkManager
{
    public async Task<string> CreateAsync(int projectId)
    {
        var project = await projectRepository.GetByIdAsync(projectId);

        if (project is null)
            throw new ProjectNotFoundException();

        var url = Guid.NewGuid().ToString("N");
        var entity = new ProjectLinkEntity
        {
            ProjectId = projectId,
            Url = url
        };
        await projectLinkRepository.CreateAsync(entity);
        return url;
    }

    public async Task<ProjectLinkModel?> GetByIdAsync(int id)
    {
        return ProjectLinkMapper.ToModel(await projectLinkRepository.GetByIdAsync(id));
    }

    public async Task<ProjectLinkModel?> GetByLinkAsync(string link)
    {
        return ProjectLinkMapper.ToModel(await projectLinkRepository.GetByLinkAsync(link));
    }
}