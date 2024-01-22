using API.Entities.Movies;

namespace API.Interfaces.Movies
{
    public interface IGenreRepository
    {
        void CreateGenre();
        void UpdateGenre();
        void DeleteGenre();
        Task<Genre> GetGenreByIdAsync(int genreId);
        Task<Genre> GetListGenreAsync(Genre genre);
    }
}
