using SpodIglyMVC.DAL;
using SpodIglyMVC.Infrastructure;
using SpodIglyMVC.Models;
using SpodIglyMVC.ViewModels;
using System;
using System.Collections.Generic;
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
            List<Album> newArrivals;

            ICasheProvider casheProvider = new DefaultCasheProvider();

            if (casheProvider.IsSet(Const.NewItemCasheKey))
                newArrivals = casheProvider.Get(Const.NewItemCasheKey) as List<Album>;
            else
            {
                newArrivals = db.Albums.Where(w => !w.IsHidden).OrderByDescending(o => o.DateAdded).Take(3).ToList();
                casheProvider.Set(Const.NewItemCasheKey, newArrivals, 30);
            }
                

            var genres = db.Genres.ToList();

           var bestSellers = db.Albums.Where(w => !w.IsHidden && w.IsBestseller).OrderBy(g => Guid.NewGuid()).Take(3).ToList();

            var vm = new HomeViewModel
            {
                Bestsellers = bestSellers,
                NewArrivals = newArrivals,
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

