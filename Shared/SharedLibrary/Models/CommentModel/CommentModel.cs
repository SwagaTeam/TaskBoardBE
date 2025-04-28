using System.Text.Json.Serialization;

public class CommentModel
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public int ItemId { get; set; }
    public string Text { get; set; } = "";
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ItemModel Item { get; set; }

    [JsonIgnore]
    public ICollection<AttachmentModel> Attachments { get; set; }
}