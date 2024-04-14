using API.Data;
using API.DTOs.Movies.Movie;
using API.DTOs.Photos;
using API.Entities.Movies;
using API.Extentions;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            if (userId == 0 || userId == null) return null;

            var user = await _userRepository.GetMemberById(userId);
            var userAge = user.DateOfBirth.CalculateAge();

            var userRatedMovieIds = await GetUserRatedMovieIds(userId);
            var userWatchedMovieIds = await GetUserWatchedMovieIds(userId);

            var query =  _dataContext.Movies
                .Include(r => r.Ratings)
                .Include(c => c.Certification)
                .Include(wl => wl.Watchlists)
                .Include(b => b.Banner)
                .Where(m => m.IsDeleted == false)
                .Where(m => m.Certification.MinimumAge <= userAge) // Lấy các phim phù hợp với tuổi của người dùng
                .OrderByDescending(m => m.Ratings.Count) // Sắp xếp theo tổng lượt đánh giá từ cao đến thấp
                .ThenByDescending(m => m.Ratings.Average(r => r.Score)) // Tiếp tục sắp xếp theo điểm trung bình từ cao đến thấp
                .ThenByDescending(m => m.ReleaseDate)
                .Select(m => new ListMoviesOutputDto
                {
                    Id = m.Id,
                    Title = m.Title,
                    AverageRating = m.Ratings.Any() ? m.Ratings.Average(r => r.Score) : 0,
                    BannerOutput = new BannerDto()
                    {
                        Id = m.Banner.Id,
                        Url = m.Banner.Url,
                    },
                    IsInWatchList = userWatchedMovieIds.Contains(m.Id),
                    TotalRatings = m.Ratings.Count
                }).AsQueryable();

            if (userWatchedMovieIds.Any())
            {
                query = query.Where(m => !userWatchedMovieIds.Contains(m.Id)); // Loại bỏ phim đã xem
            }

            if (userRatedMovieIds.Any())
            {
                query = query.Where(m => !userRatedMovieIds.Contains(m.Id)); // Loại bỏ phim đã được người dùng đánh giá
            }

            var recommendedMovies = await query.Take(10).ToListAsync();
            return recommendedMovies;
        }

        private async Task<List<int>> GetUserRatedMovieIds(int userId)
        {
            // Lấy danh sách Id của các phim đã được người dùng đánh giá
            return await _dataContext.Ratings
                .Where(r => r.AppUserId == userId)
                .Select(r => r.MovieId)
                .ToListAsync();
        }

        private async Task<List<int>> GetUserWatchedMovieIds(int userId)
        {
            // Lấy danh sách Id của các phim đã được người dùng xem
            return await _dataContext.Watchlists
                .Where(w => w.AppUserId == userId)
                .Select(w => w.MovieId)
                .ToListAsync();
        }
    }
}
