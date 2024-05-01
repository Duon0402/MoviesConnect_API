using API.Entities.Movies.Persons;

namespace API.Entities.Movies
{
        public class Movie : Entity
        {
            public string Title { get; set; }
            public string Summary { get; set; }
            public int DurationMinutes { get; set; }
            public DateTime ReleaseDate { get; set; }
            public string Status { get; set; }
            public int CertificationId { get; set; }
            public Certification Certification { get; set; }
            public ICollection<MovieGenre> MovieGenres { get; set; }
            // banner
            public Banner Banner { get; set; }
            // watchlist
            public ICollection<Watchlist> Watchlists { get; set; }
            // rating
            public ICollection<Rating> Ratings { get; set; }
            // person
            public int DirectorId { get; set; }
            public Director Director { get; set; }
            public ICollection<MovieActor> MovieActors { get; set; }
        }
    }