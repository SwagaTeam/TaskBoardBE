using System.Text.Json.Serialization;

namespace ProjectService.Models;

public class JoinRequest
{
    [JsonRequired]
    public string Url { get; set; } = default!;
    [JsonRequired]
    public int UserId { get; set; }
}
