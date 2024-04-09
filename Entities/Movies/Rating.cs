using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using API.Entities.Users;

namespace API.Entities.Movies
{
    public class Rating
    {
        public int Score { get; set; }
        public string? Review { get; set; }
        public bool IsSpoil { get; set; } = false;
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
