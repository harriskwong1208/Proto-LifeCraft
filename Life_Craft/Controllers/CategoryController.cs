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
				TempData["Success"] = "Created succesfully";

				return RedirectToAction("Index");
			}
            return View();
			/*If want to redirect to different controller, put controller name as second parameter */
			//return RedirectToAction("Index","Home");

		}
		public IActionResult Edit(int? id)
		{
			if(id ==null || id== 0)
			{
				return NotFound();
			}
			Category? obj = _db.Categories.Find(id);
			if(obj == null)
			{
				return NotFound();
			}



			return View(obj);
		}
		[HttpPost]
		public IActionResult Edit(Category obj)
		{

			if (ModelState.IsValid)
			{
				_db.Categories.Update(obj);
				_db.SaveChanges();
				TempData["Success"] = "Edited succesfully";
				return RedirectToAction("Index");
			}
			return View();
			/*If want to redirect to different controller, put controller name as second parameter */
			//return RedirectToAction("Index","Home");

		}
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			Category? obj = _db.Categories.Find(id);
			if (obj == null)
			{
				return NotFound();
			}



			return View(obj);
		}
		[HttpPost,ActionName("delete")]
		public IActionResult DeletePOST(int? id)
		{
			Category? obj = _db.Categories.Find(id);
			if(obj == null) {
				return NotFound();
			}

			_db.Categories.Remove(obj);
			_db.SaveChanges();
			TempData["Success"] = "Deleted succesfully";
			return RedirectToAction("Index");

		}

	}
}
