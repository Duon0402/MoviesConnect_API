using API.DTOs.Photos;

namespace API.DTOs.Movies.Movie
{
    public class ListMoviesOutputDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public bool IsInWatchList { get; set; }
        public BannerDto Banner { get; set; }
    }
}
