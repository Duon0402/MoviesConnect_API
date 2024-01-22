using API.DTOs.Movies;
using API.Entities.Movies;
using API.Helpers.Pagination;

namespace API.Interfaces.Movies
{
    public interface IMovieRepository
    {
        void CreateMovie(MovieDto movie);
        Task<bool> SaveAllAsync();
        Task<PagedResults<Movie>> GetPagedListMoviesAsync(MovieInputDto movieInput);
        Task<Movie> GetMovieByIdAsync(int movieId);
    }
}
