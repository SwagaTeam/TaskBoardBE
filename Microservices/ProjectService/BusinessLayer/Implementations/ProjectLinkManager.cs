using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using SharedLibrary.Dapper.DapperRepositories.Abstractions;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.BusinessLayer.Implementations;

public class ProjectLinkManager(IProjectLinkRepository projectLinkRepository, IProjectRepository projectRepository, IUserRepository userRepository)
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
        return await ProjectLinkMapper.ToModel(await projectLinkRepository.GetByIdAsync(id), userRepository);
    }

    public async Task<ProjectLinkModel?> GetByLinkAsync(string link)
    {
        return await ProjectLinkMapper.ToModel(await projectLinkRepository.GetByLinkAsync(link), userRepository);
    }
}