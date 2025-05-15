using ProjectService.BusinessLayer.Abstractions;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using ProjectService.Mapper;
using SharedLibrary.Auth;
using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models.DocumentModel;


namespace ProjectService.BusinessLayer.Implementations
{
    public class DocumentManager : IDocumentManager
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IAuth auth;
        private readonly IUserProjectRepository userProjectRepository;
        public DocumentManager(IDocumentRepository repository, IAuth auth, IUserProjectRepository userProjectRepository)
        {
            documentRepository = repository;
            this.userProjectRepository = userProjectRepository;
            this.auth = auth;
        }

        public async Task<IEnumerable<DocumentModel>> GetByProjectIdAsync(int projectId)
        {
            var userId = auth.GetCurrentUserId();

            if (userId is null ||
                userId == -1)
                throw new NotAuthorizedException();

            if (await userProjectRepository.IsUserInProject((int)userId, projectId))
                return documentRepository.GetByProjectId(projectId).Select(DocumentMapper.ToModel);

            throw new NotAuthorizedException("Пользователь не состоит в проекте, к которому прикреплён документ");
        }

        public async Task AttachDocument(IFormFile file, int projectId)
        {
            var userId = auth.GetCurrentUserId();

            if (userId is null || userId == -1)
                throw new NotAuthorizedException();

            if(!await userProjectRepository.IsUserInProject((int)userId, projectId))
                throw new NotAuthorizedException("Пользователь не находится в этом проекте");

            var docPath = Environment.GetEnvironmentVariable("DOCUMENT_STORAGE_PATH");

            if (string.IsNullOrEmpty(docPath))
                throw new Exception("Переменная окружения DOCUMENT_STORAGE_PATH не задана");

            Directory.CreateDirectory(docPath);

            var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";

            var filePath = Path.Combine(docPath, uniqueFileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            docPath = $"/documents/{uniqueFileName}";

            var documentEntity = new DocumentEntity()
            {
                AuthorId = (int)userId,
                UploadedAt = DateTime.UtcNow,
                Description = file.FileName,
                Title = uniqueFileName,
                FilePath = docPath,
                ProjectId = projectId
            };

            await documentRepository.CreateDocumentAsync(documentEntity);
        }

        public async Task DeleteDocument(int documentId)
        {
            var userId = auth.GetCurrentUserId();

            if (userId is null ||
                userId == -1)
                throw new NotAuthorizedException();

            var existingDoc = await documentRepository.GetByIdAsync(documentId);

            if (existingDoc is null)
                throw new DocumentNotFoundException();

            if (await userProjectRepository.IsUserInProject((int)userId, existingDoc.ProjectId))
                await documentRepository.DeleteDocumentAsync(documentId);
            else
                throw new NotAuthorizedException("Пользователь не состоит в проекте, к которому прикреплён документ");
        }
    }
}
