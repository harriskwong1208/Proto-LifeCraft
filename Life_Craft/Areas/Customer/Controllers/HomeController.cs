using LifeCraft.DataAccess.Repository.IRepository;
using LifeCraft.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Life_Craft.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Event> eventList = _unitOfWork.Event.GetAll(includeProperties: "Category");

            return View(eventList);
        }
        public IActionResult Details(int? id)
        {
            Event _event = _unitOfWork.Event.Get(u => u.Id==id,includeProperties: "Category");

            return View(_event);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
