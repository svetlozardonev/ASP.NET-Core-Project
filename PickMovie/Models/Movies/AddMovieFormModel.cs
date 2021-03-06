namespace PickMovie.Models.Movies
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static Data.DataConstants.Movie;
    public class AddMovieFormModel
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "The title length must be in range {2} - {1} symbols!")]
        public string Title { get; set; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(MovieDescriptionMaxLength, MinimumLength = MovieDescriptionMinLength, ErrorMessage = "The description must be in range {2} - {1} symbols!")]
        public string Description { get; set; }


        [Required(ErrorMessage = "This field is required!")]
        [StringLength(DirectorNameMaxLength, MinimumLength = DirectorNameMinLength, ErrorMessage = "Director name length must be in range {2} - {1} symbols!")]
        public string Director { get; set; }

        [Display(Name = "Cast")]
        [Required(ErrorMessage = "This field is required!")]
        [StringLength(ActorsMaxLength, MinimumLength = ActorsMinLength, ErrorMessage = "Actor names length must be between {2} - {1}!")]
        public string Actors { get; set; }

        [Display(Name = "Image URL")]
        [Required(ErrorMessage = "This field is required!")]
        [Url]
        public string ImageUrl { get; set; }

        [Display(Name = "Duration Time (in minutes)")]
        [Required(ErrorMessage = "This field is required!")]
        [StringLength(MaxDurationTime, MinimumLength = MinDurationTime, ErrorMessage = "Duration time must be between {2} - {1} minutes!")]
        public string DurationTime { get; set; }

        [Range(MovieYearMinValue, MovieYearMaxValue, ErrorMessage = "The year must be between {1} - {2}.")]
        public int Year { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public IEnumerable<MovieCategoryViewModel> Categories { get; set; }

    }
}
