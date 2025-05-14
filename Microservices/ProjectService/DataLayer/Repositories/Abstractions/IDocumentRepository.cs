using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Abstractions
{
    public interface IDocumentRepository
    {
        public Task CreateDocumentAsync(DocumentEntity document);
        public Task DeleteDocumentAsync(int documentId);
        public IQueryable<DocumentEntity?> GetByProjectId(int projectId);
        public Task<DocumentEntity?> GetByIdAsync(int id);

    }
}
