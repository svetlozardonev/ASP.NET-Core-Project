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
    using System;
    using System.Linq;
    using System.Threading.Tasks;

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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(string commentId)
        {
            var comment = await this.data.Comments.FirstOrDefaultAsync(m => m.Id == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            return View(new CommentViewModel
            {
                Id = commentId,
                Content = comment.Content,
            });
        }

        [HttpPost]
        [Authorize]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(string commentId)
        {
            var comment = await this.data.Comments.FirstOrDefaultAsync(c => c.Id == commentId);

            if (comment == null)
            {
                return NotFound();
            }

            this.data.Comments.Remove(comment);
            this.data.SaveChanges();

            return RedirectToAction("Movies", "Details");
        }
    }
}


