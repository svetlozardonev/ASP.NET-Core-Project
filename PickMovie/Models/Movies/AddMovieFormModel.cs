using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static PickMovie.Data.DataConstants;

namespace PickMovie.Models.Movies
{
    public class AddMovieFormModel
    {
        [Required(ErrorMessage = "This field is required!")]
        [StringLength(TitleMaxLength, MinimumLength = TitleMinLength, ErrorMessage = "The title length must be in range {2} - {1} symbols!")]
        public string Title { get; init; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(MovieDescriptionMaxLength, MinimumLength = MovieDescriptionMinLength, ErrorMessage = "The description must be in range {2} - {1} symbols!")]
        public string Description { get; init; }

        [Required(ErrorMessage = "This field is required!")]
        [StringLength(DirectorNameMaxLength, MinimumLength = DirectorNameMinLength, ErrorMessage = "Director name length must be in range {2} - {1} symbols!")]
        public string Director { get; init; }

        [Display(Name = "Image URL")]
        [Required(ErrorMessage = "This field is required!")]
        [Url]
        public string ImageUrl { get; init; }

        [Range(MovieYearMinValue, MovieYearMaxValue, ErrorMessage = "The year must be in range {1} - {2}.")]
        public int Year { get; init; }

        [Display(Name = "Category")]
        public int CategoryId { get; init; }

        public IEnumerable<MovieCategoryViewModel> Categories { get; set; }
    }
}
