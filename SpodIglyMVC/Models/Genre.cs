using System.Collections.Generic;

namespace SpodIglyMVC.Models
{
    public class Genre
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconFilename { get; set; }

        public ICollection<Album> Albums { get; set; }
    }
}