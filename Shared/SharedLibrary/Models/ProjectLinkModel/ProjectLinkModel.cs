using SharedLibrary.ProjectModels;

public class ProjectLinkModel
{
    public int ProjectId { get; set; }
    public string URL { get; set; } = "";
    public ProjectModel? Project;
}