using System.Text.Json.Serialization;
using SharedLibrary.ProjectModels;

namespace SharedLibrary.Models;
public class BoardModel
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; private set; } = "";
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public int ItemsCount { get; private set; }

    [JsonIgnore]
    public ProjectModel? Project { get; set; }

    [JsonIgnore]
    public ICollection<StatusModel>? Statuses { get; set; }

    [JsonIgnore]
    public ICollection<SprintModel>? Sprints { get; set; } = new List<SprintModel>();

    public void SetItemsCount(int count)
    {
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        ItemsCount = count;
    }

    public void SetProjectName(string name)
    {
        ProjectName = name;
    }
}