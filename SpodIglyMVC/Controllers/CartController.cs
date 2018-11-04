using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SpodIglyMVC.App_Start;
using SpodIglyMVC.DAL;
using SpodIglyMVC.Infrastructure;
using SpodIglyMVC.Models;
using SpodIglyMVC.ViewModels;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SpodIglyMVC.Controllers
{
    public class CartController : Controller
    {
        private ShoppingCartManager shoppingCartManager;
        private ISessionManager sessionManager { get; set; }
        private StoreContext db = new StoreContext();
        private ApplicationUserManager _userManager;

        public CartController() 
        {
            this.sessionManager = new SessionManager();
            this.shoppingCartManager = new ShoppingCartManager(this.sessionManager, db)
;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public async Task<ActionResult> Checkout()
        {
            if (Request.IsAuthenticated)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                var order = new Order
                {
                    FirstName = user.UserData.FirstName,
                    LastName = user.UserData.LastName,
                    Address = user.UserData.Address,
                    CodeAndCity = user.UserData.CodeAndCity,
                    Email = user.UserData.Email,
                    PhoneNumber = user.UserData.PhoneNumber,
                };

                return View(order);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { ReturnUrl = Url.Action("Checkout", "Cart") });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Checkout(Order order)
        {
            if (ModelState.IsValid)
            {
                var userId = User.Identity.GetUserId();

                var newOrder = shoppingCartManager.CreateOrder(order, userId);

                var user = await UserManager.FindByIdAsync(userId);
                // zrobi update ponizszej klasy bazujac na order ktory przyjdzie
                TryUpdateModel(user.UserData);
                await UserManager.UpdateAsync(user);

                shoppingCartManager.EmptyCart();

                //jezeli trafiamy tutaj przez akcje post to robimy:
                // dzieki temu ze nie zwracam View uzytkownik bedzie mial opcje powrotu do poprzedniej strony bez utraty danych
                return RedirectToAction("OrderConfirmation");
            }
            else
            {
                return View(order);
            }
        }

        public ActionResult OrderConfirmation()
        {
            return View();
        }


        // GET: Cart
        public ActionResult Index()
        {
            var cartItems = shoppingCartManager.GetCart();
            var cartTotalPrice = shoppingCartManager.GetCartTotalPrice();

            CartViewModel cartVM = new CartViewModel() { CartItems = cartItems, TotalPrice = cartTotalPrice };

            return View(cartVM);
        }

        public ActionResult AddToCart(int id)
        {
            shoppingCartManager.AddToCart(id);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int albumId)
        {
            int itemCount = shoppingCartManager.RemoveFromCart(albumId);
            int cartItemsCount = shoppingCartManager.GetCartItemsCount();
            decimal cartTotal = shoppingCartManager.GetCartTotalPrice();

            // Return JSON to process in JS
            var result = new CartRemoveViewModel
            {
                RemovedItemId = albumId,
                RemovedItemCount = itemCount,
                CartItemsCount = cartItemsCount,
                CartTotal = cartTotal
            };

            return Json(result);
        }

        public int GetCartItemsCount()
        {
            return shoppingCartManager.GetCartItemsCount();
        }
    }
}