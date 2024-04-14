namespace API.DTOs.Movies.Ratings
{
    public class RatingOutputDto
    {
        public int Score { get; set; }
        public string? Review { get; set; }
        public string Username { get; set; }
        public int AppUserId { get; set; }
    }
}
