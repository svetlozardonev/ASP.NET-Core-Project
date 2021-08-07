namespace PickMovie.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;
    public class Movie
    {
        public int Id { get; init; }

        [Required]
        [MaxLength(TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MaxLength(MovieDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        [MaxLength(DirectorNameMaxLength)]
        public string Director { get; set; }

        public string ImageUrl { get; set; }

        public int Year { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; init; }



    }
}
