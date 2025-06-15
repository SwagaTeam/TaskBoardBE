namespace SharedLibrary.UserModels;

public class UserDtoModel
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}