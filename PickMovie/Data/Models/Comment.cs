namespace PickMovie.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;
    public class Comment
    {
        [Key]
        [Required]
        public string Id { get; init; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(CommentMaxLength)]
        public string Content { get; set; }

        public DateTime CreatedOn { get; init; }

        [Required]
        public string AuthorId { get; set; }

        public User Author { get; set; }

        [Required]
        public string MovieId { get; set; }

        public Movie Movie { get; set; }

        public ICollection<UserComment> UserComments { get; set; } = new List<UserComment>();
    }
}
