using System.Text.Json.Serialization;

namespace ProjectService.Models
{
    public class GetUserItemModel
    {
        [JsonRequired]
        public int ProjectId { get; set; }
        [JsonRequired]
        public int UserId { get; set; }
    }
}
