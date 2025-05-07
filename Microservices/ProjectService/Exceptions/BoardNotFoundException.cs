namespace ProjectService.Exceptions
{
    public class BoardNotFoundException : Exception
    {
        public BoardNotFoundException(string? message = "Доска не найдена") : base(message)
        {
        }
    }
}
