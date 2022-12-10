using Microsoft.AspNetCore.Mvc;
using test.Repository.IRepository;

namespace test.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork ufw;

        public CoverTypeController(IUnitOfWork _ufw)
        {
            ufw = _ufw;
        }
        public IActionResult Index()
        {
            var obj = ufw.coverType.GetAll();
            return View(obj);
        }
    }
}
