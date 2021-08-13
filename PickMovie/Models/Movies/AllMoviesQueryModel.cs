namespace PickMovie.Models.Movies
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllMoviesQueryModel
    {
        public const int MoviesPerPage = 4;
        public string Title { get; init; }
        public IEnumerable<string> Titles { get; init; }

        [Display(Name ="Search")]
        public string SearchTerm { get; init; }
        public int CurrentPage { get; set; } = 1;

        public MoviesSorting Sorting { get; init; }

        public int CategoryId { get; init; }

        public IEnumerable<MovieListingViewModel> Movies { get; init; }
    }
}
