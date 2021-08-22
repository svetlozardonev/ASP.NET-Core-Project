using Microsoft.AspNetCore.Mvc;

namespace PickMovie.Services
{
    public interface IHelper
    {
        public string RenderRazorViewToString(Controller controller, string viewName, object model = null);
    }
}
