using ProjectService.DataLayer.Repositories.Abstractions;
using ProjectService.Exceptions;
using SharedLibrary.Entities.ProjectService;
using System.Net.Mail;

namespace ProjectService.DataLayer.Repositories.Implementations
{
    public class AttachmentRepository : IAttachmentRepository
    {
        private readonly ProjectDbContext context;
        public AttachmentRepository(ProjectDbContext context)
        {
            this.context = context;
        }
        public async Task CreateAsync(AttachmentEntity attachment)
        {
            await context.Attachments.AddAsync(attachment);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int attachmentId)
        {
            var attachment = await context.Attachments.FindAsync(attachmentId);

            if (attachment is not null)
            {
                context.Attachments.Remove(attachment);
                await context.SaveChangesAsync();
                return;
            }

            throw new AttachmentNotFoundException();
        }

        public async Task<AttachmentEntity> GetById(int attachmentId)
        {
            var attachment = await context.Attachments.FindAsync(attachmentId);

            if (attachment is not null)
            {
                return attachment;
            }

            throw new AttachmentNotFoundException();
        }

        public async Task UpdateAsync(AttachmentEntity attachment)
        {
            var entity = await context.Attachments.FindAsync(attachment.Id);

            if (entity is not null)
            {
                entity.FilePath = attachment.FilePath;
                entity.UploadedAt = attachment.UploadedAt;
                entity.CommentId = attachment.CommentId;
                return;
            }

            throw new AttachmentNotFoundException();
        }
    }
}
