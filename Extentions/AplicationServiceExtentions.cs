using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.Movies;
using API.Repositories.Movies;
using API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extentions
{
    public static class AplicationServiceExtentions
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration config)
        {
            // addscoped
                // movies
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<ICertificationRepository, CertificationRepository>();
                // users
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<LogUserActivity>();
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
