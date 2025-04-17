using Org.BouncyCastle.Asn1.Ocsp;
using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using SharedLibrary.ProjectModels;

namespace ProjectService.BusinessLayer.Implementations
{
    public class ProjectLinkManager : IProjectLinkManager
    {
        private readonly IProjectLinkRepository _projectLinkRepository;
        public ProjectLinkManager(IProjectLinkRepository projectLinkRepository)
        {
            _projectLinkRepository = projectLinkRepository;
        }
        public async Task<string> Create(int projectId)
        {
            // Генерация уникального URL
            var url = Guid.NewGuid().ToString("N");

            await _projectLinkRepository.Create(new ProjectLinkModel()
            {
                ProjectId = projectId,
                URL = url
            });

            return url;
        }

        public async Task<ProjectLinkModel?> Get(int id)
        {
            return await _projectLinkRepository.Get(id);
        }

        public async Task<ProjectLinkModel?> GetByLink(string link)
        {
            return await _projectLinkRepository.GetByLink(link);
        }
    }
}
