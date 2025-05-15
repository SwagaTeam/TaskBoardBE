using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Abstractions
{
    public interface ICommentRepository
    {
        public Task CreateAsync(CommentEntity comment);
        public Task UpdateAsync(CommentEntity comment);
        public Task DeleteAsync(int commentId);
        public Task<CommentEntity> GetById(int commentId);
        public IQueryable<CommentEntity> GetByItemId(int itemId);
    }
}
