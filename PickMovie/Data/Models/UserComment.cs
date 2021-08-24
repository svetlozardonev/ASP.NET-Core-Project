namespace PickMovie.Data.Models
{
    public class UserComment
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public string CommentId { get; set; }
        public Comment Comment { get; set; }
    }
}
