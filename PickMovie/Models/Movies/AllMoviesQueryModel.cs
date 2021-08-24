namespace PickMovie.Models.Movies
{
    using System.Collections.Generic;
    public class AllMoviesQueryModel
    {
        public const int MoviesPerPage = 8;
        public int CurrentPage { get; set; } = 1;

        public int TotalMovies { get; set; }

        public string SearchTerm { get; set; }

        public IEnumerable<MovieListingViewModel> Movies { get; set; }
    }
}
