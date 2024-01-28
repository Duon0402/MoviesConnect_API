using API.Entities.Movies;
using API.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        // movies
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; } // the loai
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<Certification> Certifications { get; set; } // chung chi

        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            base.OnModelCreating(modelBuilder);

            // movies
            modelBuilder.Entity<MovieGenre>()
                .HasKey(mg => new { mg.MovieId, mg.GenreId });

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Movie)
                .WithMany(m => m.MovieGenres)
                .HasForeignKey(mg => mg.MovieId);

            modelBuilder.Entity<MovieGenre>()
                .HasOne(mg => mg.Genre)
                .WithMany(g => g.MovieGenres)
                .HasForeignKey(mg => mg.GenreId);

            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Certification)
                .WithMany(c => c.Movies)
                .HasForeignKey(m => m.CertificationId);

            // users
            modelBuilder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            modelBuilder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();
        }
    }
}