using API.Entities.Movies;

namespace API.Interfaces.Movies
{
    public interface IGenreRepository
    {
        void CreateGenre(Genre genre);
        void UpdateGenre(Genre genre);
        void DeleteGenre(Genre genre);
        Task<bool> SaveAllAsync();
        Task<Genre> GetGenreById(int genreId);
        Task<IEnumerable<Genre>> GetListGenres();
    }
}
