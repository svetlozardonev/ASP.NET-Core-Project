namespace PickMovie.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PickMovie.Models.Users;
    using System.Linq;
    using System.Threading.Tasks;
    using TestProject.Data.Models;

    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            if (this.signInManager.IsSignedIn(this.User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new UserRegisterFormModel 
            { 
                ReturnUrl = returnUrl,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterFormModel user)
        {
            // TO DO: validation if email and username are taken

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var registeredUser = new User
            {
                UserName = user.Username,
                Email = user.Email,
            };

            var result = await this.userManager.CreateAsync(registeredUser, user.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }

                return View(user);
            }

            return RedirectToAction("Index", "Home");
        }




        [HttpGet]
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

    }
}
