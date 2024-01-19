using Microsoft.AspNetCore.Identity;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;

        // role
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}
