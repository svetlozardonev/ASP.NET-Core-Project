namespace PickMovie.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PickMovie.Data;
    using PickMovie.Data.Models;
    using PickMovie.Models.Users;
    using PickMovie.Services;
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly PickMovieDbContext data;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager,
            IHelper helper,
            PickMovieDbContext data)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.data = data;
        }

        
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Profile(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var result = new UserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar,
                Gender = user.Gender,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                AboutMe = user.AboutMe,
            };

            return View(result);
        }

        [Authorize]
        public async Task<IActionResult> BecomeCritic(string returnUrl = null)
        {
             returnUrl = returnUrl == null ? "/" : returnUrl;

            var user = await this.userManager.GetUserAsync(User);

            if (user.IsCritic)
            {
                return Redirect(returnUrl);
            }

            user.IsCritic = true;

            var critic = new Critic
            {
                UserId = user.Id,
            };

            this.data.Critics.Add(critic);
            this.data.SaveChanges();

            return Redirect(returnUrl);
        }


    }
}
