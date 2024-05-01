using API.Data;
using API.DTOs.Movies.Movie;
using API.DTOs.Photos;
using API.Entities.Movies;
using API.Interfaces.Movies;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Movies
{
    public class WatchlistRepositoy : IWatchlistRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public WatchlistRepositoy(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public void AddMovieToWatchList(Watchlist watchlist)
        {
            _dataContext.Watchlists.Add(watchlist);
        }

        public void RemoveMovieFromWatchList(Watchlist watchlist)
        {
            _dataContext.Watchlists.Remove(watchlist);
        }
        public async Task<IEnumerable<ListMoviesOutputDto>> GetListMoviesFromWatchList(int userId, int currentUserId)
        {

           return await _dataContext.Watchlists
                .Where(wl => wl.AppUserId == userId)
                .Select(wl => new ListMoviesOutputDto
                {
                    Id = wl.Movie.Id,
                    Title = wl.Movie.Title,
                    AverageRating = _dataContext.Ratings
                        .Where(r => r.MovieId == wl.Movie.Id)
                        .Select(r => (double?)r.Score) // Chuyển đổi sang kiểu double nullable
                        .Average() ?? 0, // Nếu không có giá trị, sẽ trả về 0
                    TotalRatings = _dataContext.Ratings
                        .Count(r => r.MovieId == wl.Movie.Id),

                    IsInWatchList = _dataContext.Watchlists
                        .Any(x => x.MovieId == wl.Movie.Id && x.AppUserId == currentUserId),
                    Banner = _mapper.Map<BannerDto>(wl.Movie.Banner)
                })
                .ToListAsync();
        }

        public async Task<bool> ExistWatchlistItem(int userId, int movieId)
        {
            return await _dataContext.Watchlists
                .AnyAsync(wl => wl.MovieId == movieId && wl.AppUserId == userId);
        }
    }
}
