using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using test.Models;
using test.Models.ViewModels;
using test.Repository.IRepository;
using test.Utility;

namespace test.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class ProductController : Controller
    {
        IUnitOfWork _ufw;
        IWebHostEnvironment _WebHostEnvironment;
        public ProductController(IUnitOfWork ufw,IWebHostEnvironment WebHostEnvironment)
        {
            _ufw = ufw;
            _WebHostEnvironment = WebHostEnvironment;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int id)
        {
            ProductVm vm = new ProductVm();
           
            vm.CategoryList = _ufw.category.GetAll().Select(u => new SelectListItem(){Text=u.Name, Value=u.Id.ToString()});
            vm.CoverTypeList = _ufw.coverType.GetAll().Select(u => new SelectListItem (u.Name,u.Id.ToString()));

            if (id == 0)
            {
                vm.product = new();
            }
            else
            {
                vm.product = _ufw.product.Get(id);
            }
            return View(vm); ;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVm obj,IFormFile? file)
        {
            if (ModelState.IsValid)
            {               
                if (file != null)
                {
                    string rootpath = _WebHostEnvironment.WebRootPath;
                    string newFileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(file.FileName);
                    string path = Path.Combine(@"Images\Products", newFileName + extension);

                    if (obj.product.UrlImage != null)
                    {
                        string oldPath = Path.Combine(rootpath, obj.product.UrlImage);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    using (var filestream = new FileStream(Path.Combine(rootpath, path), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    obj.product.UrlImage = path;
                }
                if (obj.product.Id == 0)
                {
                    _ufw.product.Add(obj.product);
                }
                else
                {
                    _ufw.product.Update(obj.product);
                }
                _ufw.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        #region api calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var obj = _ufw.product.GetAll("Category,CoverType");
            return Json(new { data = obj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return Json(new { succes = false});
            }
            var obj = _ufw.product.Get(id);
            var rootpath = _WebHostEnvironment.WebRootPath;
            var path = Path.Combine(rootpath,obj.UrlImage);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            _ufw.product.Remove(obj);
            _ufw.Save();
            return Json(new { sucess = true});
        }
        #endregion

    }
}
