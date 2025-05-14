namespace ProjectService.Exceptions
{
    public class DocumentNotFoundException : Exception
    {
        public DocumentNotFoundException(string? message = "Документ не найден") : base(message)
        {
        }
    }
}
