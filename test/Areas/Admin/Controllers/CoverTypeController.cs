using test.Models;
using test.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using test.Utility;
using Microsoft.AspNetCore.Authorization;

namespace test.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles=SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork ufw;

        public CoverTypeController(IUnitOfWork _ufw)
        {
            ufw = _ufw;
        }
        public IActionResult Index()
        {
            IEnumerable<CoverType> obj = ufw.coverType.GetAll();
            return View(obj);
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Add(CoverType obj)
        {
            ufw.coverType.Add(obj);
            ufw.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var obj = ufw.coverType.Get(id);
            return View(obj);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Edit(CoverType obj)
        {

            ufw.coverType.Update(obj);
            ufw.Save();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var obj = ufw.coverType.Get(id);
            ufw.coverType.Remove(obj);
            ufw.Save();
            return RedirectToAction("Index");
        }

    }
}
