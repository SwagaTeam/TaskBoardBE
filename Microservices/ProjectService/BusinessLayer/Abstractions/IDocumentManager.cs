using SharedLibrary.Models.DocumentModel;

namespace ProjectService.BusinessLayer.Abstractions
{
    public interface IDocumentManager
    {
        public Task AttachDocument(IFormFile file, int projectId);
        public Task DeleteDocument(int documentId);
        public Task<IEnumerable<DocumentModel>> GetByProjectIdAsync(int projectId);
    }
}
