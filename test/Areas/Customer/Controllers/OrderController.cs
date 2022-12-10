using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
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
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _ufw;

        public OrderController(IUnitOfWork ufw)
        {
            _ufw = ufw;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            var vm = new OrderVm
            {
                orderHeader = _ufw.orderHeader.GetFirstOrDefault(o=>o.Id==id,"ApplicationUser"),
                orderDetails = _ufw.orderDetail.GetAll(o => o.OrderHeaderId == id, "Product")
            };
            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderDetails(OrderVm vm)
        {
            var old= _ufw.orderHeader.Get(vm.orderHeader.Id);
            old.Name = vm.orderHeader.Name;
            old.PhoneNumber = vm.orderHeader.PhoneNumber;
            old.StreetAddress = vm.orderHeader.StreetAddress;
            old.City = vm.orderHeader.City;
            old.State = vm.orderHeader.State;
            old.PostalCode = vm.orderHeader.PostalCode;
            if (vm.orderHeader.Carrier != null)
            {
                old.Carrier = vm.orderHeader.Carrier;
            }
            if (vm.orderHeader.TrackingNumber != null)
            {
                old.TrackingNumber = vm.orderHeader.TrackingNumber;
            }
            _ufw.Save();
            return RedirectToAction("Details",new {id=vm.orderHeader.Id});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ProcessOrder(int id)
        {
            _ufw.orderHeader.UpdateStatus(id,SD.orderStatusInProcess,"");
            _ufw.Save();
            return RedirectToAction("Details", new {id=id}); 
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult ShipOrder(ShoppingCartVm vm)
        {
            var order = _ufw.orderHeader.Get(vm.orderHeader.Id);
            _ufw.orderHeader.UpdateStatus(vm.orderHeader.Id, SD.orderStatusShipped,"");
            order.ShippingDate = DateTime.Now;
            order.TrackingNumber = vm.orderHeader.TrackingNumber;
            order.Carrier = vm.orderHeader.Carrier;
            _ufw.Save();

            return RedirectToAction("Details", new { id = vm.orderHeader.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Cancel(ShoppingCartVm vm)
        {
            if (vm.orderHeader.PaymentStatus == SD.orderStatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Reason = RefundReasons.RequestedByCustomer,
                    PaymentIntent = vm.orderHeader.PaymentIntentId
                };

                var service = new RefundService();
                 Refund refund = service.Create(options); 

                _ufw.orderHeader.UpdateStatus(vm.orderHeader.Id, SD.orderStatusCancelled, SD.orderStatusRefunded);
            }
            else
            {
                _ufw.orderHeader.UpdateStatus(vm.orderHeader.Id, SD.orderStatusCancelled, SD.orderStatusCancelled);
            }

            _ufw.Save();
            return RedirectToAction("Details", new {id=vm.orderHeader.Id});
        }

        public IActionResult Pay(OrderVm vm)
        {
            var domain = "https://localhost:44313";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                    "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"/Customer/Order/OrderConfirmation/{vm.orderHeader.Id}",
                CancelUrl = domain + "/Customer/Order/Index",
            };
            vm.orderDetails = _ufw.orderDetail.GetAll(o => o.OrderHeaderId == vm.orderHeader.Id, "Product");
            foreach (var item in vm.orderDetails)
            {
                var sessionLineItems = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        // Provide the exact Price ID (for example, pr_1234) of the product you want to sell
                        UnitAmount = (long)(item.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        }
                    },
                    Quantity = (long)item.Price
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

        public IActionResult OrderConfirmation(int id)
        {
            var orderHeader = _ufw.orderHeader.Get(id);
            var service = new SessionService();
            var session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _ufw.orderHeader.UpdateStatus(id, orderHeader.OrderStatus, SD.paymentStatusApproved);
            }
            return RedirectToAction("Details", new { id = id });
        }

        #region api Calls

        [HttpGet]

        public IActionResult GetAll(string status)
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<OrderHeader> orderHeaders;
            if (User.IsInRole(SD.Role_Admin))
            {
                if (status != null)
                    orderHeaders = _ufw.orderHeader.GetAll(o => o.OrderStatus == status, "ApplicationUser");
                else
                    orderHeaders = _ufw.orderHeader.GetAll("ApplicationUser");
            }
            else
            {
                if (status != null)
                    orderHeaders = _ufw.orderHeader.GetAll(o => o.OrderStatus == status&&o.ApplicationUserId==claim.Value, "ApplicationUser");
                else
                    orderHeaders = _ufw.orderHeader.GetAll(o=>o.ApplicationUserId==claim.Value,"ApplicationUser");
            }
            return Json(new { data = orderHeaders });
        }

        #endregion

    }
}
