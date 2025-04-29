using SharedLibrary.ProjectModels;

public class ProjectLinkModel
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Url { get; set; } = "";
    public ProjectModel? Project;
}