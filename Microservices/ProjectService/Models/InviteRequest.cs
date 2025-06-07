using System.Text.Json.Serialization;

namespace ProjectService.Models;
public class InviteRequest
{
    [JsonRequired]
    public string Email { get; set; }
    [JsonRequired]
    public int ProjectId { get; set; }
}

