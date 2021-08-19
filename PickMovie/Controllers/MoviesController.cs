namespace TestProject.Controllers
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PickMovie.Models.Comments;
    using PickMovie.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using TestProject.Data;
    using TestProject.Data.Models;
    using TestProject.Models.Movies;

    public class MoviesController : Controller
    {
        private readonly PickMovieDbContext data;
        private readonly UserManager<User> userManager;
        private readonly ITimeWarper timeWarper;
        public MoviesController(PickMovieDbContext data,
            UserManager<User> userManager,
            ITimeWarper timeWarper)
        {
            this.data = data;
            this.userManager = userManager;
            this.timeWarper = timeWarper;
        }

        public IActionResult All([FromQuery]AllMoviesQueryModel query)
        {
            var moviesQuery = this.data.Movies.AsQueryable();

           
            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                moviesQuery = moviesQuery.Where(m =>
                m.Title.ToLower().Contains(query.SearchTerm.ToLower()) ||
                m.Director.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            var totalMovies = moviesQuery.Count();

            var movies = moviesQuery
                .Skip((query.CurrentPage - 1) * AllMoviesQueryModel.MoviesPerPage)
                .Take(AllMoviesQueryModel.MoviesPerPage)
                .OrderBy(m => m.Title)
                .Select(m => new MovieListingViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = m.Director,
                    ImageUrl = m.ImageUrl,
                    Year = m.Year,
                    Category = m.Category.Name
                }).ToList();

            query.Movies = movies;
            query.TotalMovies = totalMovies;

            return View(query);
        }

        public IActionResult Add() => View(new AddMovieFormModel
        {
            Categories = this.GetMovieCategories()
        });

        [HttpGet]
        public async Task<IActionResult> Details(string movieId)
        {

            var movie = await this.data.Movies
                .Include(m => m.Category)
                .FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                return Redirect("Movies/All");
            }
            var totalComments = this.data.Comments.Where(c => c.Id == movieId);

            return View(new MovieListingViewModel
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Director = movie.Director,
                ImageUrl = movie.ImageUrl,
                Year = movie.Year,
                Category = movie.Category.Name,
                Comments = this.data.Comments
                .Include(c => c.Author)
                .Where(c => c.MovieId == movieId)
                .Select(c => new CommentViewModel
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedOn = this.timeWarper.TimeAgo(c.CreatedOn),
                    AuthorId = c.AuthorId,
                    AuthorName = c.Author.UserName,
                    AuthorAvatarUrl = c.Author.Avatar,
                    MovieId = c.MovieId,
                }).ToList()
            });
        }

        [HttpPost]
        public IActionResult Add(AddMovieFormModel movie)
        {
            
            if (!this.data.Categories.Any(m => m.Id == movie.CategoryId))
            {
                this.ModelState.AddModelError(nameof(movie.CategoryId), "Category does not exist!");
            }

            if (!ModelState.IsValid)
            {
                movie.Categories = this.GetMovieCategories();

                return View(movie);
            }

            var currentMovie = new Movie
            {
                Title = movie.Title,
                Description = movie.Description,
                Director = movie.Director,
                ImageUrl = movie.ImageUrl,
                Year = movie.Year,
                CategoryId = movie.CategoryId
            };

            this.data.Movies.Add(currentMovie);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }
        private IEnumerable<MovieCategoryViewModel> GetMovieCategories()
            => this.data
                .Categories
                .Select(m => new MovieCategoryViewModel
                {
                    Id = m.Id,
                    Name = m.Name
                })
                .ToList();
    }
}
