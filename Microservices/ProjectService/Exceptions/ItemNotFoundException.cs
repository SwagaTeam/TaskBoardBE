namespace ProjectService.Exceptions
{
    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string? message = "Задача не найдена") : base(message)
        {
        }
    }
}
