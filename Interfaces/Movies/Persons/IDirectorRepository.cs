using API.DTOs.Movies.Directors;
using API.DTOs.Movies.Movie;
using API.Entities.Movies.Persons;

namespace API.Interfaces.Movies.Persons
{
    public interface IDirectorRepository
    {
        void CreateDirector(Director director);
        void UpdateDirector(Director director);
        void DeleteDirectorr(Director director);
        Task<bool> DirectorExits(string directorName);
        Task<bool> Save();
        Task<DirectorOutputDto> GetDirector(int directorId);
        Task<Director> GetDirectorForEdit(int directorId);
        Task<IEnumerable<DirectorOutputDto>> GetListDirectors();
        Task<IEnumerable<ListMoviesOutputDto>> GetListMoviesByDirectorId(int directorId);
    }
}
