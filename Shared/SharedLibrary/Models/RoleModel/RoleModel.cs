using System.Text.Json.Serialization;

public class RoleModel
{
    public int Id { get; set; }
    public string Role { get; set; }

    [JsonIgnore]
    public virtual ICollection<UserProjectModel> UserProjects { get; set; }
}