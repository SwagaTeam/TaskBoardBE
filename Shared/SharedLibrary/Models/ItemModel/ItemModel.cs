using System.Text.Json.Serialization;
using SharedLibrary.ProjectModels;

public class ItemModel
{
    public int Id { get; set; }
    public int? ParentId { get; set; }
    public int? ProjectId { get; set; }
    public int? ProjectItemNumber { get; set; }
    public int BusinessId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public int Priority { get; set; }
    public int? ItemTypeId { get; set; }
    public int? StatusId { get; set; }
    public bool IsArchived { get; set; }

    [JsonIgnore]
    public ItemModel? Parent { get; set; } = null;

    [JsonIgnore]
    public ICollection<ItemModel>? Children { get; set; } = null;

    [JsonIgnore]
    public ProjectModel? Project { get; set; } = null;

    [JsonIgnore]
    public ItemTypeModel? ItemType { get; set; } = null;

    [JsonIgnore]
    public StatusModel? Status { get; set; } = null;

    [JsonIgnore]
    public ICollection<CommentModel>? Comments { get; set; } = null;

    [JsonIgnore]
    public ICollection<SprintModel>? Sprints { get; set; } = null;

}