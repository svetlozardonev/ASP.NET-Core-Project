namespace PickMovie.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PickMovie.Services;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using TestProject.Data;
    using TestProject.Data.Models;
    using TestProject.Models.Movies;

    public class CommentsController : Controller
    {
        private readonly ITimeWarper timeWarper;
        private readonly PickMovieDbContext data;
        private readonly UserManager<User> userManager;
        
       
        public CommentsController(ITimeWarper timeWarper,
            PickMovieDbContext data,
            UserManager<User> userManager)
        {
            this.timeWarper = timeWarper;
            this.data = data;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(MovieListingViewModel model)
        {
            var movie = await this.data.Movies.FindAsync(model.Id);
            var user = await this.userManager.GetUserAsync(this.User);

            var characterCount = string.IsNullOrWhiteSpace(model.CommentContent) ? 0 : model.CommentContent.Length;

            if (user == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(model.CommentContent) || movie == null)
            {
                return BadRequest();
            } 

            if (characterCount == 0)
            {
                this.ModelState.AddModelError(nameof(Content), "Comments cannot be empty...");
            }

            if (characterCount > 500)
            {
                this.ModelState.AddModelError(nameof(Content), "Comments cannot be longer than 500 characters...");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            } 


            var comment = new Comment
            {
                Content = model.CommentContent,
                CreatedOn = DateTime.UtcNow.ToLocalTime(),
                AuthorId = user.Id,
                MovieId = model.Id
            };

            this.data.Comments.Add(comment);
            this.data.SaveChanges();

            return Redirect($"/Movies/Details?movieId={model.Id}");
        }
    }
}
