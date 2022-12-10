using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using test.Repository.IRepository;
using test.Utility;

namespace test.ViewComponents
{
    public class ShoppingCartViewComponent: ViewComponent
    {
        private readonly IUnitOfWork ufw;

        public ShoppingCartViewComponent(IUnitOfWork ufw)
        {
            this.ufw = ufw;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {

                if (HttpContext.Session.GetInt32(SD.SessionCart) != null)
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                else
                {
                    HttpContext.Session.SetInt32(SD.SessionCart, ufw.shoppingCart.GetAll(s=>s.ApplicationUserId==claim.Value,"").ToList().Count);
                    return View(HttpContext.Session.GetInt32(SD.SessionCart));
                }


            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }

        }

    }
}
