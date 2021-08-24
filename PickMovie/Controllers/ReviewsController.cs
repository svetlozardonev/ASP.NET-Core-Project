namespace PickMovie.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PickMovie.Data;
    using PickMovie.Data.Models;
    using PickMovie.Models.Reviews;
    using System.Linq;
    using System.Threading.Tasks;

    public class ReviewsController : Controller
    {
        private readonly PickMovieDbContext data;
        private readonly UserManager<User> userManager;
        public ReviewsController(PickMovieDbContext data,
            UserManager<User> userManager)
        {
            this.data = data;
            this.userManager = userManager;
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(AddReviewFormModel review)
        {
            if (!ModelState.IsValid)
            {
                return View(review);
            }

            var currentReview = new Review
            {
                Content = review.Content,
            };

            this.data.Reviews.Add(currentReview);
            this.data.SaveChanges();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Mine(ReviewListingViewModel model)
        {

            var review = new ReviewListingViewModel
            {
                Id = model.Id,
                Content = model.Content,
            };


            return View(review);
        }

    }
}
