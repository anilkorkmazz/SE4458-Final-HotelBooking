namespace CommentsService.DTOs
{
    public class CommentDto
    {
        public string UserName { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
