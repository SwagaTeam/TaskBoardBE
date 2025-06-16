namespace SharedLibrary.UserModels;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; }
}