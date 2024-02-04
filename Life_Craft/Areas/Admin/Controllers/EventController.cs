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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public EventController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
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
                //Find wwwRoot folder
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    //Create a guid number as the name for the file to be stored in wwwRoot folder
                    //Add the file extentsion at the end
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    //Save file to Event folder inside images folder 
                    string eventPath = Path.Combine(wwwRootPath, @"images\Event");
              
                    using (var fileStream = new FileStream(Path.Combine(eventPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    //Add image url to database
                    eventVM.Event.ImageUrl = @"\images\Event\" + filename;
                }

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
