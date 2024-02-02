using LifeCraft.DataAccess.Data;
using LifeCraft.DataAccess.Repository.IRepository;
using LifeCraft.Models;
using Microsoft.AspNetCore.Mvc;

namespace Life_Craft.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork; 
        }
        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                //Add an error and display it if this condition occurs
                ModelState.AddModelError("Name", "The display order cannot exactly match the name");
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Created succesfully";

                return RedirectToAction("Index");
            }
            return View();
            /*If want to redirect to different controller, put controller name as second parameter */
            //return RedirectToAction("Index","Home");

        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
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

                _unitOfWork.Category.Update(obj);
                _unitOfWork.Save();
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
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }



            return View(obj);
        }
        [HttpPost, ActionName("delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();

            TempData["Success"] = "Deleted succesfully";
            return RedirectToAction("Index");

        }

    }
}
