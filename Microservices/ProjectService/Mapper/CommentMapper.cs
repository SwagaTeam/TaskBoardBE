using SharedLibrary.Dapper.DapperRepositories;
using SharedLibrary.Entities.ProjectService;

namespace ProjectService.Mapper
{
    public static class CommentMapper
    {
        public static CommentEntity? ToEntity(CommentModel? model)
        {
            if (model is null)
                return null;

            return new CommentEntity
            {
                AuthorId = model.AuthorId,
                ItemId = model.ItemId,
                Text = model.Text,
                CreatedAt = model.CreatedAt
            };
        }

        public async static Task<CommentModel?> ToModel(CommentEntity? entity)
        {
            if (entity is null)
                return null;

            var model = new CommentModel
            {
                Id = entity.Id,
                AuthorId = entity.AuthorId,
                ItemId = entity.ItemId,
                Text = entity.Text,
                CreatedAt = entity.CreatedAt,
                Attachments = entity.Attachments.Select(AttachmentMapper.ToModel).ToList()
            };

            if(entity.AuthorId != null)
            {
                var user = await UserRepository.GetUser(entity.AuthorId);

                model.SetName(user.Username);
            }
            
            return model;
        }
    }
}
