namespace ProjectService.Exceptions
{
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException(string? message = "Проект не найден") : base(message) { }
    }
}
