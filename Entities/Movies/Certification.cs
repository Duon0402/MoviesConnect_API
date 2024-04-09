using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities.Movies
{
    public class Certification : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinimumAge { get; set; }
        public ICollection<Movie> Movies { get; set; }
    }
}