using test.Models;
using test.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test.Data;
using test.Models;
using Microsoft.AspNetCore.Authorization;
using test.Utility;

namespace test.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CategoryController : Controller
    {
        IUnitOfWork ufw;
        public CategoryController(IUnitOfWork _ufw)
        {
            ufw = _ufw;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> obj = ufw.category.GetAll();
            return View(obj);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "fileds must be different");
            }
            if (ModelState.IsValid)
            {
                ufw.category.Add(obj);
                ufw.Save();
                TempData["ok"] = "Created";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var obj = ufw.category.Get(id);
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "fileds must be different");
            }
            if (ModelState.IsValid)
            {
                ufw.category.Update(obj);
                ufw.Save();
                TempData["ok"] = "Updated";
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int id)
        {
            if (id == 0 || id == null)
                return NotFound();
            var obj = ufw.category.Get(id);
            ufw.category.Remove(obj);
            ufw.Save();
            TempData["ok"] = "Deleted";
            return RedirectToAction("Index");
        }

    }
}
