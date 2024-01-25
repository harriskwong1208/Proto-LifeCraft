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
            if(obj.Name == obj.DisplayOrder.ToString())
            {
                //Add an error and display it if this condition occurs
                ModelState.AddModelError("Name", "The display order cannot exactly match the name");
            }
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
				return RedirectToAction("Index");
			}
            return View();
			/*If want to redirect to different controller, put controller name as second parameter */
			//return RedirectToAction("Index","Home");

		}
	}
}
