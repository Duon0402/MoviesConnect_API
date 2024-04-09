using API.DTOs.Movies.Movie;

namespace API.Interfaces
{
    public interface IRecommendMovieService
    {
        Task<IEnumerable<ListMoviesOutputDto>> GetListRecommendMovies(int userId);
    }
}
