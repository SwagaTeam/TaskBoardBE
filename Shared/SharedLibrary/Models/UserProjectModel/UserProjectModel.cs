using SharedLibrary.ProjectModels;

public class UserProjectModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int ProjectId { get; set; }
    public int Privilege { get; set; }
    public int RoleId { get; set; }

    public ProjectModel Project { get; set; }
    public RoleModel Role { get; set; }
}