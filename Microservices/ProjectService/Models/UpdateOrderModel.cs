using System.Text.Json.Serialization;

namespace ProjectService.Models
{
    public class UpdateOrderModel
    {
        [JsonRequired]
        public int StatusId {  get; set; }
        [JsonRequired]
        public int BoardId { get; set; }
        [JsonRequired]
        public int Order { get; set; }
    }
}
