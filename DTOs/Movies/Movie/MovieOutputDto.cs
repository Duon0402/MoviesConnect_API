using API.DTOs.Movies.Certifications;
using API.DTOs.Movies.Genres;
using API.Entities.Movies;

namespace API.DTOs.Movies.Movie
{
    public class MovieOutputDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Status { get; set; }
        public IEnumerable<GenreOutputDto> Genres { get; set; }
        public CertificationOutputDto Certification { get; set; }
    }
}
