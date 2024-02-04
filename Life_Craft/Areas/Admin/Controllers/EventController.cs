using LifeCraft.DataAccess.Repository.IRepository;
using LifeCraft.Models;
using LifeCraft.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        //Method for update or insert(Create)
        //If id is present, then update, otherwise, insert(Create)
        public IActionResult Upsert(int? id)
        {
			IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().ToList().Select(u => new SelectListItem
			{
				Text = u.Name,
				Value = u.Id.ToString()
			});

            EventVM eventVM = new()
            {
                CategoryList = CategoryList,
                Event = new Event()
            };
            if (id == null || id == 0)
            {
                return View(eventVM);
            }
            else
            {
                //update
                eventVM.Event = _unitOfWork.Event.Get(u=>u.Id == id);
                return View(eventVM);
            }
        }
        //IFormFile? file is for the file upload in the create view when post method is called
        [HttpPost]
        public IActionResult Upsert(EventVM eventVM,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Event.Add(eventVM.Event);
                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                //This is to repopulate the drop down again, because the post method will not repopulate the drop down if faced with errors
				eventVM.CategoryList = _unitOfWork.Category.GetAll().ToList().Select(u => new SelectListItem
				{
					Text = u.Name,
					Value = u.Id.ToString()
				});
                return View(eventVM);
			}
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
