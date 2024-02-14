using API.Entities.Users;

namespace API.Entities.Movies
{
    public class Watchlist
    {
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
