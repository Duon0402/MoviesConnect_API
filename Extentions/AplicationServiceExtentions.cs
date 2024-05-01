using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.Movies;
using API.Interfaces.Movies.Persons;
using API.Repositories;
using API.Repositories.Movies;
using API.Repositories.Movies.Persons;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;


namespace API.Extentions
{
    public static class AplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            #region AddScoped<>
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IDirectorRepository, DirectorRepository>();
            services.AddScoped<IActorReponsitory, ActorReponsitory>();
            services.AddScoped<IPointTransactionRepository, PointTransactionRepository>();
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
            services.AddMemoryCache();
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