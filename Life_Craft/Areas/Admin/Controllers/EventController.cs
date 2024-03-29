﻿using LifeCraft.DataAccess.Repository;
using LifeCraft.DataAccess.Repository.IRepository;
using LifeCraft.Models;
using LifeCraft.Models.ViewModels;
using LifeCraft.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;

namespace Life_Craft.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

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
            List<Event> events = _unitOfWork.Event.GetAll(includeProperties:"Category").ToList();

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
              

                    if(!string.IsNullOrEmpty(eventVM.Event.ImageUrl))
                    {
                        //delete old image 
                        //Gets the path of the old image
                        var oldImagePath = Path.Combine(wwwRootPath, eventVM.Event.ImageUrl.TrimStart('\\'));
                        if(System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(eventPath, filename), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    //Add image url to database
                    eventVM.Event.ImageUrl = @"\images\Event\" + filename;
                }
                //If id ==0, its to create a new item, else it is updating
                if (eventVM.Event.Id == 0)
                {
                    _unitOfWork.Event.Add(eventVM.Event);
                }
                else
                {
                    _unitOfWork.Event.Update(eventVM.Event);
                }
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
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Event> events = _unitOfWork.Event.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = events });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
			var eventToBeDeleted = _unitOfWork.Event.Get(u => u.Id == id);
			if (eventToBeDeleted == null)
			{
				return Json(new { success = false, message = "Error while deleting" });
			}

            if (eventToBeDeleted.ImageUrl != null)
            {
                var oldImagePath =
                               Path.Combine(_webHostEnvironment.WebRootPath,
                               eventToBeDeleted.ImageUrl.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }
            }
			_unitOfWork.Event.Remove(eventToBeDeleted);
			_unitOfWork.Save();

			return Json(new { success = true, message = "Delete Successful" });
		}
	}
        #endregion
    }


