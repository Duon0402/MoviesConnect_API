using API.DTOs.Movies.Movie;
using API.Entities.Movies;

namespace API.Interfaces.Movies
{
    public interface IWatchlistRepository
    {
        void AddMovieToWatchList(Watchlist watchlist);
        void RemoveMovieFromWatchList(Watchlist watchlist);
        Task<bool> ExistWatchlistItem(int userId, int movieId);
        Task<IEnumerable<ListMoviesOutputDto>> GetListMoviesFromWatchList(int userId);
    }
}
