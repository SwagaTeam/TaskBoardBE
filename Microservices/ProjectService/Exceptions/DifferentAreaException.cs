namespace ProjectService.Exceptions
{
    public class DifferentAreaException : Exception
    {
        public DifferentAreaException(string? message = "Данные находятся в не связанных местах") : base(message)
        {
        }
    }
}
