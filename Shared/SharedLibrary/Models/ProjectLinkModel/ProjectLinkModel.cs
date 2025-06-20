using SharedLibrary.ProjectModels;
namespace SharedLibrary.Models;

public class ProjectLinkModel
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Url { get; set; } = "";
    public ProjectModel? Project;
    public int UserId { get; set; }
}