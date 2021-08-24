namespace PickMovie.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    public class ReviewsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
