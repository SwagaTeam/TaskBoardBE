using SharedLibrary.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ProjectService.Models;

public class CreateItemModel
{
    [JsonRequired]
    public ItemModel Item { get; set; }
    [JsonRequired]
    public int BoardId { get; set; }
}