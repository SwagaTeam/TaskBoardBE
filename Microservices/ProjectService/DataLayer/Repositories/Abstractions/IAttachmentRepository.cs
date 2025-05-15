using SharedLibrary.Entities.ProjectService;

namespace ProjectService.DataLayer.Repositories.Abstractions
{
    public interface IAttachmentRepository
    {
        public Task CreateAsync(AttachmentEntity attachment);
        public Task UpdateAsync(AttachmentEntity attachment);
        public Task DeleteAsync(int attachmentId);
        public Task<AttachmentEntity> GetById(int attachmentId);
    }
}
