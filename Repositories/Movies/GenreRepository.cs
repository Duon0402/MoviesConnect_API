using API.Data;
using API.Entities.Movies;
using API.Interfaces.Movies;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Movies
{
    public class GenreRepository : IGenreRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public GenreRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public  void CreateGenre(Genre genre)
        {
            _dataContext.Genres.Add(genre);
        }

        public void DeleteGenre(Genre genre)
        {
            _dataContext.Entry(genre).State = EntityState.Modified;
        }

        public async Task<Genre> GetGenreById(int genreId)
        {
            return await _dataContext.Genres
                .SingleOrDefaultAsync(g => g.Id == genreId && g.IsDeleted == false);
        }

        public async Task<IEnumerable<Genre>> GetListGenres()
        {
            return await _dataContext.Genres
                .Where(g => g.IsDeleted == false)
                .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public void UpdateGenre(Genre genre)
        {
            _dataContext.Entry(genre).State = EntityState.Modified;
        }
    }
}
