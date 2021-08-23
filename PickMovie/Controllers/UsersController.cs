namespace PickMovie.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using PickMovie.Models.Users;
    using PickMovie.Services;
    using TestProject.Data;
    using TestProject.Data.Models;
    using static PickMovie.Services.Helper;

    // using TestProject.Models.Users;
    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly PickMovieDbContext data;
        private readonly IHelper helper;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager,
            IHelper helper,
            PickMovieDbContext data)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.helper = helper;
            this.data = data;
        }

        [NoDirectAccess]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            if (this.signInManager.IsSignedIn(this.User))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new UserLoginFormModel
            {
                ReturnUrl = returnUrl,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserLoginFormModel model)
        {
            const string invalidCredentials = "Oops! Invalid credentials!";

            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var appliedError = false;

            var existingUser = await this.data.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            if (existingUser == null)
            {
                ModelState.AddModelError(string.Empty, invalidCredentials);
                appliedError = true;
            }

            var passwordIsValid = await this.userManager.CheckPasswordAsync(existingUser, model.Password);

            if (!passwordIsValid && !appliedError) ModelState.AddModelError(string.Empty, invalidCredentials);

            if (!ModelState.IsValid)
            {
                return Json(new { isValid = false, html = helper.RenderRazorViewToString(this, "Login", model) });
            }

            var returnUrl = model.ReturnUrl == null ? Url.Content("/") : Url.Content(model.ReturnUrl);

            await this.signInManager.SignInAsync(existingUser, true);

            return Json(new { isValid = true, redirectUrl = Url.Content(returnUrl) });

        }

        [NoDirectAccess]
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

            var returnUrl = user.ReturnUrl == null ? "/" : user.ReturnUrl;

            if (!ModelState.IsValid)
            {
                return Json(new { isValid = false, html = helper.RenderRazorViewToString(this, "Register", user) });
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

                return Json(new { isValid = false, html = helper.RenderRazorViewToString(this, "Register", user) });
            }

            await this.signInManager.SignInAsync(registeredUser, true);
            return Json(new { isValid = true, redirectUrl = Url.Content(returnUrl) });
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {

            
            var ReturnUrl = returnUrl == null ? Url.Content("/") : returnUrl;
            await signInManager.SignOutAsync();
            return Redirect(ReturnUrl);
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


    }
}
