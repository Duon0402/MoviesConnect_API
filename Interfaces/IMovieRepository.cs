using API.DTOs.Movies;
using API.Entities;
using API.Helpers.Pagination;

namespace API.Interfaces
{
    public interface IMovieRepository
    {
        void CreateMovie(MovieDto movie);
        Task<bool> SaveAllAsync();
        Task<PagedResults<Movie>> GetListMoviesAsync(MovieInput movieInput);
        Task<Movie> GetMovieByIdAsync(int movieId);
    }
}
