namespace PickMovie.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PickMovie.Models.Users;
    using PickMovie.Services;
    using TestProject.Data.Models;
    using static PickMovie.Services.Helper;

    // using TestProject.Models.Users;

    public class UsersController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IHelper helper;

        public UsersController(UserManager<User> userManager, SignInManager<User> signInManager,
            IHelper helper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.helper = helper;
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
        public async Task<IActionResult> Login(UserLoginFormModel loginModel)
        {
            var returnUrl = loginModel.ReturnUrl == null ? "/" : loginModel.ReturnUrl;


            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return Json(new { isValid = true, redirectUrl = Url.Content(returnUrl) });
                }
                
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                    return Json(new { isValid = false, html = helper.RenderRazorViewToString(this, "Login", loginModel) });
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return Json(new { isValid = false, html = helper.RenderRazorViewToString(this, "Login", loginModel) });
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
