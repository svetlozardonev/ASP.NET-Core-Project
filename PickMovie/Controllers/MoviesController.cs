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
                CategoryId = movie.CategoryId
            };

            this.data.Movies.Add(currMovie);
            this.data.SaveChanges();
            
            return RedirectToAction("Index", "Home");
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
