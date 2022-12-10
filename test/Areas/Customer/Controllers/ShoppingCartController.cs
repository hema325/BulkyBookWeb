using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using test.Models;
using test.Models.ViewModels;
using test.Repository.IRepository;
using test.Utility;

namespace test.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _ufw;
        public ShoppingCartController(IUnitOfWork ufw)
        {
            _ufw = ufw;
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            ShoppingCartVm vm = new() {
                orderHeader = new(),
                cartList = _ufw.shoppingCart.GetAll(s => s.ApplicationUserId == claim.Value, "product")
            };
            foreach (var item in vm.cartList)
            {
                item.price = getPrice(item.count, item.product.price, item.product.price50, item.product.price100);
                vm.orderHeader.OrderTotal += item.price * item.count;

            }
            return View(vm);
        }

        public IActionResult Details(int Productid)
        {
            ShoppingCart obj = new()
            {
                count = 1,
                product = _ufw.product.GetFirstOrDefault(p => p.Id == Productid, "CoverType,Category")
            };
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Details(ShoppingCart obj)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            obj.ApplicationUserId = claim.Value;
            var old = _ufw.shoppingCart.GetFirstOrDefault(s => s.ApplicationUserId == claim.Value && s.ProductId == obj.ProductId);
            if (old == null)
            {
                _ufw.shoppingCart.Add(obj);
                _ufw.Save();
                HttpContext.Session.SetInt32(SD.SessionCart, _ufw.shoppingCart.GetAll().ToList().Count);
            }
            else
            {
                _ufw.shoppingCart.Increment(old, obj.count);
                _ufw.Save();
            }
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Plus(int id)
        {
            ShoppingCart obj = _ufw.shoppingCart.Get(id);
            _ufw.shoppingCart.Increment(obj, 1);
            _ufw.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int id)
        {
            ShoppingCart obj = _ufw.shoppingCart.Get(id);
            if (obj.count > 1)
                _ufw.shoppingCart.Decrement(obj, 1);
            else
                _ufw.shoppingCart.Remove(obj);
            _ufw.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int id)
        {
            ShoppingCart obj = _ufw.shoppingCart.Get(id);
            _ufw.shoppingCart.Remove(obj);
            _ufw.Save();
            HttpContext.Session.SetInt32(SD.SessionCart, _ufw.shoppingCart.GetAll().ToList().Count);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVm obj = new()
            {
                cartList = _ufw.shoppingCart.GetAll(s => s.ApplicationUserId == claim.Value, "product"),
                orderHeader = new()
                {
                    ApplicationUser = _ufw.applicationUser.GetFirstOrDefault(a => a.Id == claim.Value),
                    OrderTotal = 0
                }
            };
            obj.orderHeader.Name = obj.orderHeader.ApplicationUser.Name;
            obj.orderHeader.PostalCode = obj.orderHeader.ApplicationUser.PostalCode;
            obj.orderHeader.State = obj.orderHeader.ApplicationUser.State;
            obj.orderHeader.City = obj.orderHeader.ApplicationUser.City;
            obj.orderHeader.StreetAddress = obj.orderHeader.ApplicationUser.StreetAddress;
            obj.orderHeader.PhoneNumber = obj.orderHeader.ApplicationUser.PhoneNumber;
            foreach (var item in obj.cartList)
            {
                item.price = getPrice(item.count, item.product.price, item.product.price50, item.product.price100);
                obj.orderHeader.OrderTotal += item.price * item.count;
            }

            return View(obj);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]

        public IActionResult SummaryPost(ShoppingCartVm vm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var appUser = _ufw.applicationUser.GetFirstOrDefault(a => a.Id == claim.Value);
            vm.orderHeader.ApplicationUserId = claim.Value;

            var user = _ufw.applicationUser.GetFirstOrDefault(a => a.Id == claim.Value);

            if (user.CompanyId.GetValueOrDefault() == 0)
            {
                vm.orderHeader.PaymentStatus = SD.paymentStatusPending;
                vm.orderHeader.OrderStatus = SD.orderStatusPending;
            }
            else
            {
                vm.orderHeader.PaymentStatus = SD.paymentStatusDelayedPayment;
                vm.orderHeader.OrderStatus = SD.orderStatusApproved;
            }
            vm.orderHeader.OrderDate = DateTime.Now;
               

            vm.cartList = _ufw.shoppingCart.GetAll(s => s.ApplicationUserId == claim.Value, "product");
            foreach(var item in vm.cartList)
            {
                item.price = getPrice(item.count, item.product.price, item.product.price50, item.product.price100);
                vm.orderHeader.OrderTotal += item.count * item.price;
            }

            _ufw.orderHeader.Add(vm.orderHeader);
            _ufw.Save();

            foreach(var item in vm.cartList)
            {
                OrderDetail obj = new()
                {
                    ProductId = item.ProductId,
                    OrderHeaderId = vm.orderHeader.Id,
                    Count = item.count,
                    Price = item.price
                };
                _ufw.orderDetail.Add(obj);
                _ufw.Save();
            }

            if (user.CompanyId.GetValueOrDefault()==0) { 

            var domain = "https://localhost:44313";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems=new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"/Customer/ShoppingCart/OrderConfirmation/{vm.orderHeader.Id}",
                CancelUrl = domain + "/Customer/ShoppingCart/Index",
            };

            foreach (var item in vm.cartList)
            {
                var sessionLineItems = new SessionLineItemOptions
                {
                    PriceData =new SessionLineItemPriceDataOptions
                    {
                        // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                        UnitAmount = (long)(item.price * 100),
                        Currency="usd",
                        ProductData=new SessionLineItemPriceDataProductDataOptions
                        {
                            Name=item.product.Title
                        }
                    },
                    Quantity = (long)item.price
                };
                options.LineItems.Add(sessionLineItems);

            }

            var service = new SessionService();
            Session session = service.Create(options);
            _ufw.orderHeader.UpdateStripePaymentId(vm.orderHeader.Id, session.Id, session.PaymentIntentId);
            _ufw.Save();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            }
            return RedirectToAction("OrderConfirmation", new { id = vm.orderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            var orderHeader = _ufw.orderHeader.GetFirstOrDefault(o => o.Id == id);

            if (orderHeader.PaymentStatus != SD.paymentStatusDelayedPayment) {
                var sessionService = new SessionService();
                var session = sessionService.Get(orderHeader.SessionId);

                if (session.PaymentStatus.ToLower() == "paid")
                {
                    _ufw.orderHeader.UpdateStatus(id, SD.orderStatusApproved, SD.paymentStatusApproved);
                    _ufw.Save();
                }
            }
            List<ShoppingCart> cartList = _ufw.shoppingCart.GetAll(s => s.ApplicationUserId == orderHeader.ApplicationUserId,"product").ToList();
            _ufw.shoppingCart.RemoveRange(cartList);
            _ufw.Save();
            HttpContext.Session.Clear();

            return View(id);
        }
        private double getPrice(int count,double price,double price50,double price100)
        {
            if (count < 50)
                return price;
            if (count < 100)
                return price50;
            return price100;
        }

    }
}
