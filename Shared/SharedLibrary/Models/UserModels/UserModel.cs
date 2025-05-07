namespace SharedLibrary.UserModels
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ImagePath { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
    }
}
