namespace ProjectService.Models
{
    public class NewCommentModel
    {
        public CommentModel comment { get; set; }
        public IFormFile attachment { get; set; }
    }
}
