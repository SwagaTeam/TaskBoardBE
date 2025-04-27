namespace ProjectService.Exceptions
{
    public class NotAuthorizedException : Exception
    {
        public NotAuthorizedException(string? message = "Не авторизованный доступ") : base(message) { }

    }
}
