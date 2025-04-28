using System.Text.Json.Serialization;

public class AttachmentModel
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public int CommentId { get; set; }
    public string FilePath { get; set; }
    public DateTime UploadedAt { get; set; }

    [JsonIgnore]
    public CommentModel Comment { get; set; }
}