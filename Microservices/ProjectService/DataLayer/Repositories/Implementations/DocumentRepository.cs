using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using SharedLibrary.Entities.ProjectService;
using System.Linq;

namespace ProjectService.DataLayer.Repositories.Implementations
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ProjectDbContext context;
        public DocumentRepository(ProjectDbContext context)
        {
            this.context = context;
        }

        public async Task<DocumentEntity?> GetByIdAsync(int id)
        {
            return await context.Documents.FindAsync(id);
        }

        public IQueryable<DocumentEntity?> GetByProjectId(int projectId)
        {
            return context.Documents.Where(x => x.ProjectId == projectId);
        }

        public async Task CreateDocumentAsync(DocumentEntity document)
        {
            await context.AddAsync(document);
            await context.SaveChangesAsync();
        }

        public async Task DeleteDocumentAsync(int documentId)
        {
            var doc = await context.Documents.FindAsync(documentId);

            if(doc is not null)
            {
                context.Documents.Remove(doc);
                return;
            }

            throw new DocumentNotFoundException();
            
        }
    }
}
