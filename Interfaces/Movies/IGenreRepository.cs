using API.DTOs.Movies.Genres;
using API.Entities.Movies;
using API.Helpers.Pagination;

namespace API.Interfaces.Movies
{
    public interface IGenreRepository
    {
        void CreateGenre(Genre genre);
        void UpdateGenre(Genre genre);
        void DeleteGenre(Genre genre);
        Task<bool> GenreExits(string genreName);
        Task<bool> Save();
        Task<Genre> GetGenreByIdForEdit(int genreId);
        Task<GenreOutputDto> GetGenreById(int genreId);
        Task<PagedResults<GenreOutputDto>> GetListGenres(GenreInputDto genreInput);
    }
}
