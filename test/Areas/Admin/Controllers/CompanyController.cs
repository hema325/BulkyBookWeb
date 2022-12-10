using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using test.Models;
using test.Repository.IRepository;
using test.Utility;

namespace test.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _ufw;

        public CompanyController (IUnitOfWork ufw)
        {
            _ufw = ufw;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int id)
        {
            if (id != 0)
            {
                var obj = _ufw.company.Get(id);
                return View(obj);
            }
            return View(new Company());
        }

        [HttpPost]

        public IActionResult Upsert(Company obj)
        {
            if (obj.Id == 0)
            {
                _ufw.company.Add(obj);
                TempData["ok"] = "Added";
            }
            else
            {
                _ufw.company.Update(obj);
                TempData["ok"] = "Updated";
            }
            _ufw.Save();
            return RedirectToAction("Index");
        }

        #region api calls
        public IActionResult GetAll()
        {
            var obj = _ufw.company.GetAll();
            return Json(new { data = obj });
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return Json(new { success = false });
            }
            var obj = _ufw.company.Get(id);
            _ufw.company.Remove(obj);
            _ufw.Save();
            TempData["ok"] = "Deleted";
            return Json(new { success = true });
        }

        #endregion

    }
}
