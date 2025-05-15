using Microsoft.EntityFrameworkCore;
using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using SharedLibrary.Entities.ProjectService;
using System.Net.Mail;

namespace ProjectService.DataLayer.Repositories.Implementations
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ProjectDbContext context;
        public CommentRepository(ProjectDbContext context)
        {
            this.context = context;
        }
        public async Task CreateAsync(CommentEntity comment)
        {
            await context.Comments.AddAsync(comment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int commentId)
        {
            var comment = await context.Comments.FindAsync(commentId);

            if (comment is not null)
            {
                context.Comments.Remove(comment);
                await context.SaveChangesAsync();
                return;
            }

            throw new CommentNotFoundException();
        }

        public async Task<CommentEntity> GetById(int commentId)
        {
            var comment = await context.Comments.FindAsync(commentId);

            if (comment is not null)
            {
                return comment;
            }

            throw new CommentNotFoundException();
        }

        public IQueryable<CommentEntity> GetByItemId(int itemId)
        {
            var comments = context.Comments
                                    .Include(x=>x.Attachments)
                                    .Where(x => x.ItemId == itemId);

            return comments;
        }

        public async Task UpdateAsync(CommentEntity comment)
        {
            var entity = await context.Comments.FindAsync(comment.Id);

            if (entity is not null)
            {
                
                return;
            }

            throw new CommentNotFoundException();
        }
    }
}
