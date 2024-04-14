using API.Entities;
using API.Entities.Movies;
using API.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Data
{
    public static class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return;

            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "seed_users.json");
            var json = File.ReadAllText(filePath);
            var users = JsonConvert.DeserializeObject<List<AppUser>>(json);
            if (users == null) return;

            var roles = new List<AppRole>
            {
                new AppRole { Name = "Member" },
                new AppRole { Name = "Admin" },
                new AppRole { Name = "Moderator" },
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();
                user.IsPrivate = false; // Thêm thông tin IsPublic
                user.Avatar = new Avatar
                {
                    PublicId = "default_avatar",
                    Url = "https://res.cloudinary.com/dspm3zys2/image/upload/v1707741814/user_yxfmyc.png"
                }; // Thêm thông tin Avatar
                await userManager.CreateAsync(user, "D@ngDuong0402");
                await userManager.AddToRoleAsync(user, "Member");
            }
        }

        public static async Task SeedAdmin(UserManager<AppUser> userManager)
        {
            var admin = new AppUser
            {
                UserName = "admin",
                FullName = "Admin",
                Gender = "Male",
                DateOfBirth = new DateTime(2002, 04, 02),
                IsPrivate = true,
                Avatar = new Avatar
                {
                    PublicId = "default_avatar",
                    Url = "https://res.cloudinary.com/dspm3zys2/image/upload/v1707741814/user_yxfmyc.png"
                }
            };

            await userManager.CreateAsync(admin, "D@ngDuong0402");
            await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
        }

        #region Genres
        public static async Task SeedGenres(DataContext context)
        {
            if (await context.Genres.AnyAsync()) return;

            var genres = new List<Genre>
            {
                new Genre { Name = "Action" },
                new Genre { Name = "Adventure" },
                new Genre { Name = "Animation" },
                new Genre { Name = "Biography" },
                new Genre { Name = "Comedy" },
                new Genre { Name = "Crime" },
                new Genre { Name = "Documentary" },
                new Genre { Name = "Drama" },
                new Genre { Name = "Family" },
                new Genre { Name = "Fantasy" },
                new Genre { Name = "Film-Noir" },
                new Genre { Name = "History" },
                new Genre { Name = "Horror" },
                new Genre { Name = "Music" },
                new Genre { Name = "Musical" },
                new Genre { Name = "Mystery" },
                new Genre { Name = "Romance" },
                new Genre { Name = "Sci-Fi" },
                new Genre { Name = "Short" },
                new Genre { Name = "Sport" },
                new Genre { Name = "Thriller" },
                new Genre { Name = "War" },
                new Genre { Name = "Western" }
            };

            await context.Genres.AddRangeAsync(genres);
            await context.SaveChangesAsync();
        }
        #endregion

        #region Certifications
        public static async Task SeedCertifications(DataContext context)
        {
            if (await context.Certifications.AnyAsync()) return;

            var certifications = new List<Certification>
            {
                new Certification { Name = "P", Description = "This film is suitable for all audiences, with no age restrictions.", MinimumAge = 0 },
                new Certification { Name = "C13", Description = "Recommended for audiences over 13 years old.", MinimumAge = 13 },
                new Certification { Name = "C16", Description = "Recommended for audiences over 16 years old.", MinimumAge = 16 },
                new Certification { Name = "C18", Description = "Film restricted to audiences over 18 years old.", MinimumAge = 18 }
            };

            await context.Certifications.AddRangeAsync(certifications);
            await context.SaveChangesAsync();
        }
        #endregion

        public static async Task SeedMovies(DataContext context)
        {
            if (await context.Movies.AnyAsync()) return;

            // Sử dụng đường dẫn tuyệt đối đến thư mục Data
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "seed_movies.json");

            // Đọc dữ liệu từ tệp seed_movies.json
            var json = File.ReadAllText(filePath);
            var movies = JsonConvert.DeserializeObject<List<Movie>>(json);

            foreach (var movie in movies)
            {
                context.Movies.Add(movie);
            }

            await context.SaveChangesAsync();
        }
    }
}