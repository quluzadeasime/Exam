using Business.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ExamApp.Controllers
{
    public class HomeController : Controller
    {
        IChefService _chefService;

        public HomeController(IChefService chefService)
        {
            _chefService = chefService;
        }

        public IActionResult Index()
        {
            var chefs = _chefService.GetAll();
            return View(chefs);
        }

    }
}
