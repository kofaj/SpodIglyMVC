using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SpodIglyMVC.Models
{
    public class Album
    {
        public int AlbumId { get; set; }
        public int GenreId { get; set; }

        [Display(Name = "Tytuł")]
        [Required]
        public string AlbumTitle { get; set; }
        
        [Display(Name = "Artysta")]
        [DataType(DataType.Text)]
        [Required]
        public string ArtistName { get; set; }
        public DateTime DateAdded { get; set; }
        public string CoverFileName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsBestseller { get; set; }

        public virtual Genre Genre { get; set; }
        public bool IsHidden { get; set; }
    }
}