using API.Data;
using API.DTOs.Movies.Movie;
using API.DTOs.Photos;
using API.Extentions;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace API.Services
{
    public class RecommendMovieService : IRecommendMovieService
    {
        private readonly DataContext _dataContext;
        private readonly IUserRepository _userRepository;
        private readonly IMemoryCache _cache;

        public RecommendMovieService(DataContext dataContext, IUserRepository userRepository, IMemoryCache cache)
        {
            _dataContext = dataContext;
            _userRepository = userRepository;
            _cache = cache;
        }

        public async Task<IEnumerable<ListMoviesOutputDto>> GetListRecommendMovies(int userId)
        {
            if (userId == 0 || userId == null)
                return null;

            var cacheKey = $"RecommendedMovies_{userId}";
            if (_cache.TryGetValue(cacheKey, out IEnumerable<ListMoviesOutputDto> recommendedMovies))
            {
                return recommendedMovies;
            }

            recommendedMovies = await CalculateRecommendedMovies(userId);
            _cache.Set(cacheKey, recommendedMovies, TimeSpan.FromMinutes(5));

            return recommendedMovies;
        }

        private async Task<IEnumerable<ListMoviesOutputDto>> CalculateRecommendedMovies(int userId)
        {
            var user = await _userRepository.GetMemberById(userId);
            var userAge = user.DateOfBirth.CalculateAge();

            var userRatedMovieIds = await GetUserRatedMovieIds(userId);
            var userWatchedMovieIds = await GetUserWatchedMovieIds(userId);

            var query = _dataContext.Movies
                .AsNoTracking()
                .Where(m => !m.IsDeleted && m.Certification.MinimumAge <= userAge && m.Status == "Released")
                .OrderByDescending(m => m.Ratings.Count)
                .ThenByDescending(m => m.Ratings.Average(r => r.Score))
                .ThenByDescending(m => m.ReleaseDate)
                .Select(m => new
                {
                    m.Id,
                    m.Title,
                    m.Ratings,
                    m.ReleaseDate,
                    m.Banner,
                });

            if (userWatchedMovieIds.Any())
                query = query.Where(m => !userWatchedMovieIds.Contains(m.Id));

            if (userRatedMovieIds.Any())
                query = query.Where(m => !userRatedMovieIds.Contains(m.Id));

            var recommendedMovies = await query
                .Take(12)
                .Select(m => new ListMoviesOutputDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    AverageRating = m.Ratings.Any() ? m.Ratings.Average(r => r.Score) : 0,
                    Banner = new BannerDto
                    {
                        Id = m.Banner.Id,
                        Url = m.Banner.Url
                    },
                    IsInWatchList = userWatchedMovieIds.Contains(m.Id),
                    TotalRatings = m.Ratings.Count
                }).ToListAsync();

            return recommendedMovies;
        }

        private async Task<List<int>> GetUserRatedMovieIds(int userId)
        {
            return await _dataContext.Ratings
                .AsNoTracking()
                .Where(r => r.AppUserId == userId)
                .Select(r => r.MovieId)
                .ToListAsync();
        }

        private async Task<List<int>> GetUserWatchedMovieIds(int userId)
        {
            return await _dataContext.Watchlists
                .AsNoTracking()
                .Where(w => w.AppUserId == userId)
                .Select(w => w.MovieId)
                .ToListAsync();
        }
    }
}
