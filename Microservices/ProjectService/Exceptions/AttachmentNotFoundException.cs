namespace ProjectService.Exceptions
{
    public class AttachmentNotFoundException : Exception
    {
        public AttachmentNotFoundException(string? message = "Прикрепляемый файл не найден") : base(message)
        {
        }
    }
}
