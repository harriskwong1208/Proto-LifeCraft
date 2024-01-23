using Microsoft.AspNetCore.Mvc;

namespace Life_Craft.Controllers
{
    public class CategoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
