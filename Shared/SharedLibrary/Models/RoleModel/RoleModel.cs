using System.Text.Json.Serialization;

namespace SharedLibrary.Models;

public class RoleModel
{
    public int Id { get; set; }
    public string Role { get; set; } = string.Empty;

    [JsonIgnore]
    public virtual ICollection<UserProjectModel> UserProjects { get; set; } = new List<UserProjectModel>();
}