namespace PickMovie.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PickMovie.Data;
    using PickMovie.Models;
    using PickMovie.Views.Home;
    using System.Diagnostics;
    using System.Linq;
    using TestProject.Data;
    public class HomeController : Controller
    {
        private readonly PickMovieDbContext db;
        public HomeController(PickMovieDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            var viewModel = new IndexViewModel
            {
                MoviesCount = db.Movies.Count(),
                CategoriesCount = db.Categories.Count(),
                UsersCount = db.Users.Count(),
                CommentsCount = db.Comments.Count(),
                
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
