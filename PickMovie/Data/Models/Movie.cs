namespace PickMovie.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants.Movie;
    public class Movie
    {
        [Key]
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(MovieDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(DirectorNameMaxLength)]
        public string Director { get; set; }

        [Required]
        public string Actors { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        [Required]
        public string DurationTime { get; set; }

        [Required]
        public int Year { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; init; }

        public ICollection<Review> Reviews { get; init; } = new List<Review>();
        public ICollection<Comment> Comments { get; init; } = new List<Comment>();
        public ICollection<UserMovie> UserMovies { get; set; } = new List<UserMovie>();


    }
}
