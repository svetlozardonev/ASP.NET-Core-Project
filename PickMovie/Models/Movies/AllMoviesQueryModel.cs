namespace PickMovie.Models.Movies
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class AllMoviesQueryModel
    {
        public IEnumerable<string> Titles { get; init; }

        [Display(Name ="Search")]
        public string SearchTerm { get; init; }
        public MoviesSorting Sorting { get; init; }

        public IEnumerable<MovieListingViewModel> Movies { get; init; }
    }
}
