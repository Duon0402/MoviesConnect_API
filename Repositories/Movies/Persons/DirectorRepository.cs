using API.Data;
using API.DTOs.Movies.Directors;
using API.DTOs.Movies.Movie;
using API.Entities.Movies.Persons;
using API.Interfaces.Movies.Persons;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Movies.Persons
{
    public class DirectorRepository : IDirectorRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public DirectorRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public void CreateDirector(Director director)
        {
            _dataContext.Directors.Add(director);
        }

        public void DeleteDirectorr(Director director)
        {
            _dataContext.Entry(director).State = EntityState.Modified;
        }

        public async Task<bool> DirectorExits(string directorName)
        {
            return await _dataContext.Directors
                .AnyAsync(a => a.Name == directorName && a.IsDeleted == false);
        }

        public async Task<DirectorOutputDto> GetDirector(int directorId)
        {
            return await _dataContext.Directors
                .Where(d => d.Id == directorId & d.IsDeleted == false)
                .ProjectTo<DirectorOutputDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<Director> GetDirectorForEdit(int directorId)
        {
            return await _dataContext.Directors
                .Where(d => d.Id == directorId)
                .Include(d => d.DirectorImage)
                .SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<DirectorOutputDto>> GetListDirectors()
        {
            return await _dataContext.Directors
                .Where(d => d.IsDeleted == false)
                .ProjectTo<DirectorOutputDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<IEnumerable<ListMoviesOutputDto>> GetListMoviesByDirectorId(int directorId)
        {
            return await _dataContext.Directors.Where(d => d.Id == directorId)
                .Select(d => d.Movies)
                .ProjectTo<ListMoviesOutputDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public void UpdateDirector(Director director)
        {
            _dataContext.Entry(director).State = EntityState.Modified;
        }
    }
}
