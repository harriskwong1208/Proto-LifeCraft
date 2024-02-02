using LifeCraft.DataAccess.Repository.IRepository;
using LifeCraft.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Life_Craft.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EventController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public EventController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Event> events = _unitOfWork.Event.GetAll().ToList();

            return View(events);
        }
        public IActionResult Edit(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Event? obj = _unitOfWork.Event.Get(u => u.Id == id);

            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);


        }
        [HttpPost]
        public IActionResult Edit(Event obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Event.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Edited succesfully";
                return RedirectToAction("Index");
            }


            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Event obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Event.Add(obj);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == 0 || id == null)
            {
                return NotFound();
            }
            Event? obj = _unitOfWork.Event.Get(u => u.Id == id);
            return View(obj);
        }
        [HttpPost, ActionName("delete")]
        public IActionResult DeletePost(int? id)
        {
            Event? obj = _unitOfWork.Event.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Event.Remove(obj);
            _unitOfWork.Save();
            return RedirectToAction("Index");


        }
    }
}
