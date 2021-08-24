namespace PickMovie.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PickMovie.Data;
    using PickMovie.Data.Models;
    using PickMovie.Models.Comments;
    using PickMovie.Models.Movies;
    using PickMovie.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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

        public async Task<IActionResult> All([FromQuery]AllMoviesQueryModel query)
        {
            var moviesQuery = this.data.Movies.AsQueryable();

            var user = await this.userManager.GetUserAsync(this.User);
            var userId = user == null ? null : user.Id;
           
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
                    Category = m.Category.Name,
                    IsLiked = isLiked(m.Id, userId, this.data),
                }).ToList();

            query.Movies = movies;
            query.TotalMovies = totalMovies;

            return View(query);
        }

        [Authorize]
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
                DurationTime = movie.DurationTime,
                Actors = movie.Actors,
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
        [Authorize]
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
                CategoryId = movie.CategoryId,
                Actors = movie.Actors,
                DurationTime = movie.DurationTime,
            };

            this.data.Movies.Add(currentMovie);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string movieId)
        {
            var movie = await this.data.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                return NotFound();
            }

            return View(new AddMovieFormModel
            {
                Id = movieId,
                Title = movie.Title,
                Description = movie.Description,
                Director = movie.Director,
                ImageUrl = movie.ImageUrl,
                Year = movie.Year,
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(AddMovieFormModel model)
        {
            var movie = await this.data.Movies.FirstOrDefaultAsync(m => m.Id == model.Id);

            if (movie == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            movie.Title = model.Title;
            movie.Description = model.Description;
            movie.Director = model.Director;
            movie.ImageUrl = model.ImageUrl;
            movie.Year = model.Year;

            await this.data.SaveChangesAsync();

            return Redirect($"/Movies/Details?movieId={movie.Id}");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(string movieId)
        {
            var movie = await this.data.Movies.FirstOrDefaultAsync(m => m.Id == movieId);

            if (movie == null)
            {
                return NotFound();
            }

            return View(new MovieListingViewModel 
            { 
                Id = movieId,
            });
        }

        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string movieId)
        {
            var movie = await this.data.Movies.FirstOrDefaultAsync(m => m.Id == movieId);
            var comments = this.data.Comments.Where(c => c.MovieId == movieId);

            if (movie == null)
            {
                return NotFound();
            }

            this.data.Comments.RemoveRange(comments);
            this.data.Movies.Remove(movie);
            this.data.SaveChanges();

            return RedirectToAction(nameof(All));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Like(string movieId)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var usermovie = await this.data.UserMovies.FirstOrDefaultAsync(um => um.MovieId == movieId && um.UserId == user.Id);

            var isLiked = usermovie != null;

            if (isLiked)
            {
                this.data.UserMovies.Remove(usermovie);
            }
            else
            {
                usermovie = new UserMovie
                {
                    UserId = user.Id,
                    MovieId = movieId
                };

                this.data.UserMovies.Add(usermovie);
            }

            await this.data.SaveChangesAsync();

            return Redirect("/Movies/All/");
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

        private static bool isLiked(string movieId, string userId, PickMovieDbContext data)
        {
            if (userId == null) return false;
            
            var result = data.UserMovies.FirstOrDefault(um => um.MovieId == movieId && um.UserId == userId) != null;

            return result;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Favourites([FromQuery] AllMoviesQueryModel query)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var moviesQuery = this.data.Movies
                .Include(m => m.UserMovies)
                .Where(m => m.UserMovies.Any(um => um.UserId == user.Id))
                .AsQueryable();

            var userId = user == null ? null : user.Id;

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
                    Category = m.Category.Name,
                    IsLiked = isLiked(m.Id, userId, this.data),
                }).ToList();

            query.Movies = movies;
            query.TotalMovies = totalMovies;

            return View(query);
        }
    }
}
