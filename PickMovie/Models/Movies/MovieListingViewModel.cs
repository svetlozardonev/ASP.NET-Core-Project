namespace PickMovie.Models.Movies
{
    public class MovieListingViewModel
    {
        public int Id { get; init; }
        public string Title { get; init; }
        public string Description { get; init; }    
        public string Director { get; init; }
        public string ImageUrl { get; init; }
        public int Year { get; init; }
        public string Category { get; init; }
    }
}
