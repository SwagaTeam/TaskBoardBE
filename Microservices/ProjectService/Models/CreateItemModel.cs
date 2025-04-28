using System.ComponentModel.DataAnnotations;

namespace ProjectService.Models;

public class CreateItemModel
{
    public ItemModel Item { get; set; }
    [Required]
    public int BoardId { get; set; }
}