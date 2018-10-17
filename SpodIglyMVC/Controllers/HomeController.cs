using SpodIglyMVC.DAL;
using SpodIglyMVC.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SpodIglyMVC.Controllers
{
    public class HomeController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Home
        public ActionResult Index()
        {
            var genres = db.Genres.ToList();
            var newArrials = db.Albums.Where(w => !w.IsHidden).OrderByDescending(o => o.DateAdded).Take(3).ToList();
            var bestSellers = db.Albums.Where(w => !w.IsHidden && w.IsBestseller).OrderBy(g => Guid.NewGuid()).Take(3).ToList();

            var vm = new HomeViewModel
            {
                Bestsellers = bestSellers,
                NewArrivals = newArrials,
                Genres = genres
            };

            return View(vm);
        }

        public ActionResult Details()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }
    }
}

