using System.ComponentModel;
using System.Text.Json.Serialization;
using SharedLibrary.ProjectModels;
namespace SharedLibrary.Models;

public class ItemModel
{
    public int Id { get; set; }
    [DefaultValue(null)]
    public int? ParentId { get; set; }
    public int? ProjectId { get; set; }
    public int? BoardId { get; private set; }
    public int? ProjectItemNumber { get; set; }
    public string BusinessId { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public int Priority { get; set; }
    public string PriorityText => SharedLibrary.Constants.Priority.Names[Priority];
    public int? ItemTypeId { get; set; }
    public int? StatusId { get; set; }
    public bool IsArchived { get; set; }

    [JsonInclude]
    public List<ContributorModel> Contributors { get; private set; } = new List<ContributorModel>();
    public string Author { get; private set; } = "";

    public void AddContributor(ContributorModel contributor)
    {
        Contributors.Add(contributor);
    }

    public void SetAuthor(string author) 
    { 
        Author = author; 
    }

    public void SetBoardId(int? boardId)
    {
        BoardId = boardId;
    }

    [JsonIgnore]
    public ItemModel? Parent { get; set; } = null;

    [JsonIgnore]
    public ICollection<ItemModel>? Children { get; set; } = null;

    [JsonIgnore]
    public ProjectModel? Project { get; set; } = null;

    [JsonIgnore]
    public ItemTypeModel? ItemType { get; set; } = null;

    [JsonIgnore]
    public ICollection<UserItemModel>? UserItems { get; set; }

    public StatusModel? Status { get; set; } = null;

    [JsonIgnore]
    public ICollection<CommentModel>? Comments { get; set; } = null;

    [JsonIgnore]
    public ICollection<SprintModel>? Sprints { get; set; } = null;

}