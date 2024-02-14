using API.Data;
using API.DTOs.Movies.Movie;
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
        public async Task<IEnumerable<ListMoviesOutputDto>> GetListMoviesFromWatchList(int userId)
        {
           return await _dataContext.Watchlists
                .Where(wl => wl.AppUserId == userId)
                .Select(wl => wl.Movie)
                .ProjectTo<ListMoviesOutputDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> ExistWatchlistItem(int userId, int movieId)
        {
            return await _dataContext.Watchlists
                .AnyAsync(wl => wl.MovieId == movieId && wl.AppUserId == userId);
        }
    }
}
