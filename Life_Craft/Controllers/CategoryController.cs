using Life_Craft.Data;
using Life_Craft.Models;
using Microsoft.AspNetCore.Mvc;

namespace Life_Craft.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db) { 
            _db = db;
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            _db.Categories.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");

            /*If want to redirect to different controller, put controller name as second parameter */
			//return RedirectToAction("Index","Home");

		}
	}
}
