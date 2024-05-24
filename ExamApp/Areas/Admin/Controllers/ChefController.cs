using Business.Exceptions;
using Business.Services.Abstracts;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class ChefController : Controller
    {
        IChefService _chefService;

        public ChefController(IChefService chefService)
        {
            _chefService = chefService;
        }

        public IActionResult Index()
        {
            var chefs = _chefService.GetAll();
            return View(chefs);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Chef chef)
        {
            if (!ModelState.IsValid) return View();

            try
            {
                _chefService.Add(chef);
            }
            catch(FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName,ex.Message);
                return View();
            }
            catch (ContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (PhotoNullException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (ChefNullException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            if(!ModelState.IsValid) return View();
            var existChef = _chefService.Get(x=>x.Id == id);
            try
            {
                _chefService.Delete(id);
            }
            catch(Business.Exceptions.FileNotFoundException ex)
            {
                return NotFound();
            }
            catch(ChefNotFoundException ex)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Update(int id)
        {
            var existChef = _chefService.Get(x => x.Id == id);
            if(existChef == null) return NotFound();
            return View(existChef);
        }

        [HttpPost]
        public async Task<IActionResult> Update(Chef chef)
        {
            if(!ModelState.IsValid) return View(); 
            try
            {
                _chefService.Update(chef.Id, chef);
            }
            catch (FileSizeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (ContentTypeException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (ChefNullException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (ChefNotFoundException ex)
            {
                ModelState.AddModelError(ex.PropertyName, ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
