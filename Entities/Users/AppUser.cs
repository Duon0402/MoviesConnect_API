using API.Entities.Movies;
using Microsoft.AspNetCore.Identity;

namespace API.Entities.Users
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public ICollection<AppUserRole> UserRoles { get; set; }
        public Avatar Avatar { get; set; }
        public ICollection<Rating> Ratings { get; set; }
        public ICollection<Watchlist> Watchlists { get; set; }
    }
}