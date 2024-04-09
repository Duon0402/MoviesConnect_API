using API.Entities;
using API.Entities.Movies;
using API.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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

        #region DbSet<>
        public DbSet<Report> Reports { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Watchlist> Watchlists { get; set; }
        public DbSet<Avatar> Avatars { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; } // the loai
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<Certification> Certifications { get; set; } // chung chi
        #endregion
        protected override void OnModelCreating(ModelBuilder modelBuilder)

        {
            base.OnModelCreating(modelBuilder);

            #region Ratings
            modelBuilder.Entity<Rating>()
                .HasKey(r => new { r.MovieId, r.AppUserId });
            modelBuilder.Entity<Rating>()
                .HasOne(u => u.AppUser)
                .WithMany(r => r.Ratings)
                .HasForeignKey(u => u.AppUserId);
            modelBuilder.Entity<Rating>()
                .HasOne(m => m.Movie)
                .WithMany(r => r.Ratings)
                .HasForeignKey(m => m.MovieId);
            #endregion

            #region Watchlists
            modelBuilder.Entity<Watchlist>()
                .HasKey(wl => new { wl.MovieId, wl.AppUserId });

            modelBuilder.Entity<Watchlist>()
                .HasOne(u => u.AppUser)
                .WithMany(wl => wl.Watchlists)
                .HasForeignKey(u => u.AppUserId);
            modelBuilder.Entity<Watchlist>()
                .HasOne(m => m.Movie)
                .WithMany(wl => wl.Watchlists)
                .HasForeignKey(m => m.MovieId);
            #endregion

            #region Photos
            modelBuilder.Entity<Avatar>()
                .HasOne(u => u.AppUser)
                .WithOne(a => a.Avatar)
                .HasForeignKey<Avatar>(u => u.AppUserId)
                .IsRequired();

            modelBuilder.Entity<Banner>()
                .HasOne(m => m.Movie)
                .WithOne(b => b.Banner)
                .HasForeignKey<Banner>(m => m.MovieId)
                .IsRequired();

            #endregion

            #region Movies
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

            #endregion

            #region Users
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
            #endregion
        }
    }
}