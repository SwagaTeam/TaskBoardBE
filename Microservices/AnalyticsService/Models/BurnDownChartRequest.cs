using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AnalyticsService.Models
{
    public class BurnDownChartRequest
    {
        [JsonRequired]
        public int ProjectId { get; set; }
        [JsonRequired]
        public int Priority { get; set; }
    }
}
