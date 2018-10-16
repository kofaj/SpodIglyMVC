using SpodIglyMVC.Models;
using System.Collections.Generic;

namespace SpodIglyMVC.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Album> Bestsellers { get; set; }
        public IEnumerable<Album> NewArrivals { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
    }
}