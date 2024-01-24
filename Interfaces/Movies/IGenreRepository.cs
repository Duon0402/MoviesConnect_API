using API.Entities.Movies;

namespace API.Interfaces.Movies
{
    public interface IGenreRepository
    {
        void CreateGenre();
        void UpdateGenre();
        void DeleteGenre();
        Task<Genre> GetGenreById(int genreId);
        Task<Genre> GetListGenre(Genre genre);
    }
}
