using SpodIglyMVC.DAL;
using System.Linq;
using System.Web.Mvc;

namespace SpodIglyMVC.Controllers
{
    public class StoreController : Controller
    {
        private StoreContext db = new StoreContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var album = db.Albums.Find(id);

            return View(album);
        }

        public ActionResult List(string genrename)
        {
            var genre = db.Genres.Include("Albums").Where(w => w.Name.ToUpper() == genrename.ToUpper()).Single();
            var albums = genre.Albums.ToList();
            return View(albums);
        }

        // ponizszy atrybut blokuje mozliwosc odpalenia akcji bezposrednio z http
        // output cashe zapisuje wynik pierwszego zapytania i pozniej to nam zwraca (standardowy cashe)
        // Duration - liczy w sekundach
        [ChildActionOnly]
        [OutputCache(Duration = 80000)]
        public ActionResult GenresMenu()
        {
            var genreList = db.Genres.ToList();

            return PartialView("_GenresMenu", genreList);
        }
    }
}