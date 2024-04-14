using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using API.Entities.Users;

namespace API.Entities.Movies
{
    public class Rating
    {
        public int Score { get; set; }
        public string? Review { get; set; }
        public bool RatingViolation { get; set; } = false;
        public int? DeletedId { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
