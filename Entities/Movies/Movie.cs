using API.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities.Movies
{
    public class Movie
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        //public string Storyline { get; set; }
        //public DateTime ReleaseDate { get; set; }

        //public ICollection<Genre> Genres { get; set; }
    }
}
