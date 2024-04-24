using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.Movies;
using API.Repositories;
using API.Repositories.Movies;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions
{
    public static class AplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            #region AddScoped<>
            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<IReportRepository, ReportRepository>();
            services.AddScoped<IRecommendMovieService, RecommendMovieService>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IWatchlistRepository, WatchlistRepositoy>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddScoped<IPhotoService, PhotoService>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<ICertificationRepository, CertificationRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<LogUserActivity>();
            #endregion
            // automaper
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);
            // database
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            });

            return services;
        }
    }
}