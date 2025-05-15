namespace ProjectService.Exceptions
{
    public class CommentNotFoundException : Exception
    {
        public CommentNotFoundException(string? message = "Комментарий не найден") : base(message)
        {
        }
    }
}
