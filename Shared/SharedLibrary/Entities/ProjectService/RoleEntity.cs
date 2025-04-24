using System.Text.Json.Serialization;

namespace SharedLibrary.Entities.ProjectService;

public class RoleEntity
{
    public int Id { get; set; }
    public string Role { get; set; }
    [JsonIgnore]
    public virtual ICollection<UserProjectEntity> UserProjects { get; set; }
}