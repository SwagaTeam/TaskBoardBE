namespace ProjectService.Exceptions
{
    public class StatusNotFoundException : Exception
    {
        public StatusNotFoundException(string? message = "Статус не найден") : base(message)
        {
        }
    }
}
