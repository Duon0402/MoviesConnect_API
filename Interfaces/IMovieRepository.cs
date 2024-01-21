using API.DTOs.Movies;
using API.Entities;

namespace API.Interfaces
{
    public interface IMovieRepository
    {
        void CreateMovie(MovieDto movie);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<Movie>> GetListMoviesAsync();
        Task<Movie> GetMovieByIdAsync(int movieId);
    }
}
