namespace PickMovie.Models.Comments
{
    public class CommentViewModel
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string CreatedOn { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorAvatarUrl { get; set; }
        public string MovieId { get; set; }
    }
}
