namespace API.DTOs.Movies.Ratings
{
    public class RatingOutputDto
    {
        public int Id { get; set; }
        public int Score { get; set; }
        public string? Comment { get; set; }
        public int AppUserId { get; set; }
    }
}
