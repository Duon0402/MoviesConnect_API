using API.Data;
using API.DTOs.Movies.Movie;
using API.DTOs.Photos;
using API.Entities.Movies;
using API.Extentions;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class RecommendMovieService : IRecommendMovieService
    {
        private readonly DataContext _dataContext;
        private readonly IUserRepository _userRepository;

        public RecommendMovieService(DataContext dataContext, IUserRepository userRepository)
        {
            _dataContext = dataContext;
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<ListMoviesOutputDto>> GetListRecommendMovies(int userId)
        {
            if (userId == 0 || userId == null)
                return null;

            var user = await _userRepository.GetMemberById(userId);
            var userAge = user.DateOfBirth.CalculateAge();

            var userRatedMovieIds = await GetUserRatedMovieIds(userId);
            var userWatchedMovieIds = await GetUserWatchedMovieIds(userId);

            var query = _dataContext.Movies
                .AsNoTracking()
                .Where(m => !m.IsDeleted && m.Certification.MinimumAge <= userAge)
                .OrderByDescending(m => m.Ratings.Count)
                .ThenByDescending(m => m.Ratings.Average(r => r.Score))
                .ThenByDescending(m => m.ReleaseDate)
                .Select(m => new ListMoviesOutputDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    AverageRating = m.Ratings.Any() ? m.Ratings.Average(r => r.Score) : 0,
                    BannerOutput = new BannerDto
                    {
                        Id = m.Banner.Id,
                        Url = m.Banner.Url
                    },
                    IsInWatchList = userWatchedMovieIds.Contains(m.Id),
                    TotalRatings = m.Ratings.Count
                })
                .AsQueryable();

            if (userWatchedMovieIds.Any())
                query = query.Where(m => !userWatchedMovieIds.Contains(m.Id));

            if (userRatedMovieIds.Any())
                query = query.Where(m => !userRatedMovieIds.Contains(m.Id));

            var recommendedMovies = await query.Take(12).ToListAsync();
            return recommendedMovies;
        }

        private async Task<List<int>> GetUserRatedMovieIds(int userId)
        {
            return await _dataContext.Ratings
                .Where(r => r.AppUserId == userId)
                .Select(r => r.MovieId)
                .ToListAsync();
        }

        private async Task<List<int>> GetUserWatchedMovieIds(int userId)
        {
            return await _dataContext.Watchlists
                .Where(w => w.AppUserId == userId)
                .Select(w => w.MovieId)
                .ToListAsync();
        }
    }
}