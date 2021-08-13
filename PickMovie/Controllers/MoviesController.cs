namespace PickMovie.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PickMovie.Data;
    using PickMovie.Data.Models;
    using PickMovie.Models.Movies;
    using System.Collections.Generic;
    using System.Linq;

    public class MoviesController : Controller
    {
        private readonly PickMovieDbContext data;

        public MoviesController(PickMovieDbContext data) 
            => this.data = data;

        public IActionResult Add() => View(new AddMovieFormModel
        {
            Categories = this.GetMovieCategories()
        });

        public IActionResult All([FromQuery]AllMoviesQueryModel query)
        {
            var moviesQuery = this.data.Movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Title))
            {
                moviesQuery = moviesQuery.Where(m => m.Title == query.Title);
            }

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                moviesQuery = moviesQuery.Where(m =>
                    m.Title.ToLower().Contains(query.SearchTerm.ToLower()) ||
                    m.Director.ToLower().Contains(query.SearchTerm.ToLower()));
            }

            var movies = moviesQuery
                .Skip((query.CurrentPage - 1) * AllMoviesQueryModel.MoviesPerPage)
                .Take(AllMoviesQueryModel.MoviesPerPage)
                .Select(m => new MovieListingViewModel
                {
                    Id = m.Id,
                    Title = m.Title,
                    Director = m.Director,
                    ImageUrl = m.ImageUrl,
                    Year = m.Year,
                    Category = m.Category.Name
                })
                .ToList();

            return View(query);
        }

        [HttpPost]
        public IActionResult Add(AddMovieFormModel movie)
        {
            if (!this.data.Categories.Any(m => m.Id == movie.CategoryId))
            {
                this.ModelState.AddModelError(nameof(movie.CategoryId), "Category");
            }

            if (!ModelState.IsValid)
            {
                movie.Categories = this.GetMovieCategories();

                return View(movie);

            }

            var currMovie = new Movie
            {
                Title = movie.Title,
                Description = movie.Description,
                Director = movie.Director,
                ImageUrl = movie.ImageUrl,
                Year = movie.Year,
                CategoryId = movie.CategoryId
            };

            this.data.Movies.Add(currMovie);
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
