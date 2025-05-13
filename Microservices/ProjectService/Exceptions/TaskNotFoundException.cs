namespace ProjectService.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(string? message = "Задача не найдена") : base(message)
        {
        }
    }
}
