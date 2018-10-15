using SpodIglyMVC.DAL;
using SpodIglyMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SpodIglyMVC.Controllers
{
    public class HomeController : Controller
    {
        private StoreContext db = new StoreContext();

        // GET: Home
        public ActionResult Index()
        {
            var genreList = db.Genres.ToList();

            return View();
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

