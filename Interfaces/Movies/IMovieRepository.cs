using API.DTOs.Movies.Genres;
using API.DTOs.Movies.Movie;
using API.Entities.Movies;
using API.Helpers.Pagination;
using API.Helpers.Params.Movies;

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

        Task<Movie> GetListMoviesForEdit(MovieParams movieParams);

        Task<MovieOutputDto> GetMovieById(int movieId);
        Task<IEnumerable<ListMoviesOutputDto>> GetListMovies(MovieParams movieParams);
        Task<IPagedResult<MovieOutputDto>> GetPagedListMovies(MovieInputDto movieInput);
        Task<IEnumerable<GenreOutputDto>> GetListGenresByMovieId(int movieId);
    }
}