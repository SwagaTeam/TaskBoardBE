using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models.DocumentModel;

namespace ProjectService.Mapper
{
    public static class DocumentMapper
    {
        public static DocumentEntity? ToEntity(DocumentModel model)
        {
            if (model is null)
                return null;
            return new DocumentEntity 
            { 
                AuthorId = model.AuthorId,
                UploadedAt = model.UploadedAt,
                Description = model.Description,
                FilePath = model.FilePath,
                ProjectId = model.ProjectId,
                Title = model.Title
            };
        }

        public static DocumentModel? ToModel(DocumentEntity entity)
        {
            if (entity is null)
                return null;
            return new DocumentModel
            {
                Id = entity.Id,
                AuthorId = entity.AuthorId,
                UploadedAt = entity.UploadedAt,
                Description = entity.Description,
                FilePath = entity.FilePath,
                ProjectId = entity.ProjectId,
                Title = entity.Title
            };
        }
    }
}
