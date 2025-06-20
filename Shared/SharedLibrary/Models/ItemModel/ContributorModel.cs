namespace SharedLibrary.Models;
public class ContributorModel
{
    public string UserName { get; set; }
    public string ImagePath { get; set; }

    public ContributorModel(string userName, string imagePath)
    {
        UserName = userName;
        ImagePath = imagePath;
    }
}