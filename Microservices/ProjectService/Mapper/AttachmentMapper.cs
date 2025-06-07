using SharedLibrary.Entities.ProjectService;
using SharedLibrary.Models;

namespace ProjectService.Mapper
{
    public static class AttachmentMapper
    {
        public static AttachmentEntity? ToEntity(AttachmentModel? model)
        {
            if (model is null)
                return null;

            return new AttachmentEntity
            {
                AuthorId = model.AuthorId,
                CommentId = model.CommentId,
                UploadedAt = model.UploadedAt,
                FilePath = model.FilePath
            };
        }

        public static AttachmentModel? ToModel(AttachmentEntity? entity)
        {
            if (entity is null)
                return null;

            return new AttachmentModel
            {
                Id = entity.Id,
                AuthorId = entity.AuthorId,
                CommentId = entity.CommentId,
                UploadedAt = entity.UploadedAt,
                FilePath= entity.FilePath
            };
        }
    }
}
