namespace ProjectService.Exceptions
{
    public class SprintNotFoundException : Exception
    {
        public SprintNotFoundException(string? message = "Спринт не найден") : base(message)
        {
        }
    }
}
