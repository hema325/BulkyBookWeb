using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using test.Models;
using test.Repository.IRepository;

namespace test.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _ufw;
        public HomeController(ILogger<HomeController> logger,IUnitOfWork ufw)
        {
            _logger = logger;
            _ufw = ufw;
        }

        public IActionResult Index()
        {
            var obj = _ufw.product.GetAll("CoverType,Category");
            return View(obj);
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