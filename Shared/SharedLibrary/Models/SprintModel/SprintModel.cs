using System.Text.Json.Serialization;
using SharedLibrary.Entities.ProjectService;

namespace SharedLibrary.Models;

public class SprintModel
{
    public int Id { get; set; }
    public int BoardId { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ICollection<ItemModel>? Items { get; set; }

    [JsonIgnore]
    public BoardEntity? Board { get; set; }
}