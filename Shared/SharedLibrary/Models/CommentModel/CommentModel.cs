using System.Text.Json.Serialization;

public class CommentModel
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public int ItemId { get; set; }
    public string Text { get; set; } = "";
    public string Name { get; private set; } = "";
    public DateTime CreatedAt { get; set; }

    [JsonIgnore]
    public ItemModel Item { get; set; }

    public ICollection<AttachmentModel> Attachments { get; set; }

    public void SetName(string name)
    {
        Name = name;
    }
}