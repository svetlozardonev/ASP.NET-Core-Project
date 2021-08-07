namespace PickMovie.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using PickMovie.Data;
    using PickMovie.Models;
    using PickMovie.Models.Home;
    using PickMovie.Models.Movies;

    public class HomeController : Controller
    {
        private readonly PickMovieDbContext data;

        public HomeController(PickMovieDbContext data)
            => this.data = data;
        public IActionResult Index()
        {
            var totalMovies = this.data.Movies.Count();
            var movies = this.data
                .Movies
                .OrderByDescending(m => m.Id)
                .Select(m => new MovieIndexViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = m.Director,
                    ImageUrl = m.ImageUrl,
                    Year = m.Year
                })
                .Take(3)
                .ToList();

            return View(new IndexViewModel
            {
                TotalMovies = totalMovies,
                Movies = movies
            });
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
