using API.Entities;
using API.Entities.Movies;
using API.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public static class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var roles = new List<AppRole>
            {
                new AppRole{Name = "Member"},
                new AppRole{Name = "Admin"},
                new AppRole{Name = "Moderator"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            var admin = new AppUser
            {
                UserName = "admin",
                FullName = "Dang Truong Duong",
                Gender = "Male",
                DateOfBirth = new DateTime(2002, 04, 02),
                IsPublic = false,
                Avatar = new Avatar
                {
                    AppUserId = 1,
                    PublicId = "default_avatar",
                    Url = "https://res.cloudinary.com/dspm3zys2/image/upload/v1707741814/user_yxfmyc.png"
                }
            };

            await userManager.CreateAsync(admin, "D@ngDuong0402");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
        }

        public static async Task SeedCertifications(DataContext context)
        {
            if (await context.Certifications.AnyAsync()) return;

            var certifications = new List<Certification>
            {
                new Certification { Name = "P", Description = "Phim này phù hợp với mọi đối tượng khán giả, không giới hạn độ tuổi.", MinimumAge = 0 },
                new Certification { Name = "C13", Description = "Khuyến cáo đối tượng khán giả trên 13 tuổi.", MinimumAge = 13 },
                new Certification { Name = "C16", Description = "Khuyến cáo đối tượng khán giả trên 16 tuổi.", MinimumAge = 16 },
                new Certification { Name = "C18", Description = "Phim chỉ dành cho khán giả trên 18 tuổi.", MinimumAge = 18 }
            };

            await context.Certifications.AddRangeAsync(certifications);
            await context.SaveChangesAsync();
        }
    }
}