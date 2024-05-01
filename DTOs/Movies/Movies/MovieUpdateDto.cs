namespace API.DTOs.Movies.Movie
{
    public class MovieUpdateDto
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public int DurationMinutes { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string Status { get; set; }
        public int CertificationId { get; set; }
        public List<int> GenreIds { get; set; }
        public int DirectorId { get; set; }
        public List<int> ActorIds { get; set; }
    }
}