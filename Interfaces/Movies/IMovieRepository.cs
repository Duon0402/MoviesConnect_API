using API.DTOs.Movies;
using API.Entities.Movies;
using API.Helpers.Pagination;

namespace API.Interfaces.Movies
{
    public interface IMovieRepository
    {
        void CreateMovie(Movie movie);

        void UpdateMovie(Movie movie);

        void DeleteMovie(Movie movie);

        void ApproveMovie(Movie movie);

        Task<bool> Save();

        Task<bool> MovieExits(string movieTitle);

        Task<Movie> GetMovieByIdForEdit(int movieId);

        Task<Movie> GetListMoviesForEdit(string orderBy, string purpose);

        Task<MovieOutputDto> GetMovieById(int movieId);

        Task<IPagedResult<MovieOutputDto>> GetListMovies(MovieInputDto movieInput);
    }
}