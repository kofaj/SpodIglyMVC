
using SpodIglyMVC.ViewModels;
using System.Web.Mvc;

namespace SpodIglyMVC.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        // returnUrl - app zapisze strone na ktora chcial sie dostac uzytkownik i po udanym logowaniu przekieruje go na nia
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
        
        // akcja bedzie wywolana tylko i wylacznie po POST
        [HttpPost]
        public ActionResult Login(LoginViewModels viewModel, string returnUrl)
        {
            if(!ModelState.IsValid)
            {
                // jezeli model bedzi  e zawierac bledy to poinformuje o tym uzytkownika i zwroci mu to co wpisal
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }
            else
            {
                return RedirectToAction("index", "Home"); 
            }
        }
    }
}