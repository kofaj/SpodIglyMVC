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

        public ActionResult List(string genrename, string searchQuery = null)
        {
            var genre = db.Genres.Include("Albums").Where(w => w.Name.ToUpper() == genrename.ToUpper()).Single();

            var albums = genre.Albums.Where(w => (searchQuery == null ||
                                        w.AlbumTitle.ToLower().Contains(searchQuery.ToLower()) ||
                                        w.ArtistName.ToLower().Contains(searchQuery.ToLower())) && !w.IsHidden);
            //sprawdzanie z jakim typem request mamy do czynienia (zwykly/ajax)
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProductList", albums);
            }


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

        public ActionResult AlbumsSuggestions(string term)
        {
            // select jest tylko po to aby zwracać lablelki (wymóg jquert ui)
            var album = this.db.Albums.Where(w => !w.IsHidden && w.AlbumTitle.ToLower().Contains(term.ToLower())).
                Take(5).Select(s => new { label = s.AlbumTitle });

            return Json(album, JsonRequestBehavior.AllowGet);
        }
    }
}