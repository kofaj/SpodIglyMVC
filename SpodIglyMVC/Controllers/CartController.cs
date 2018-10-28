using SpodIglyMVC.DAL;
using SpodIglyMVC.Infrastructure;
using SpodIglyMVC.ViewModels;
using System.Web.Mvc;

namespace SpodIglyMVC.Controllers
{
    public class CartController : Controller
    {
        private ShoppingCartManager shoppingCartManager;
        private ISessionManager sessionManager { get; set; }
        private StoreContext db = new StoreContext();

        public CartController() 
        {
            this.sessionManager = new SessionManager();
            this.shoppingCartManager = new ShoppingCartManager(this.sessionManager, db)
;
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