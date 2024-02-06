using API.Data;
using API.DTOs.Movies;
using API.Entities.Movies;
using API.Helpers.Pagination;
using API.Interfaces.Movies;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Movies
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public MovieRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public void ApproveMovie(Movie movie)
        {
            _dataContext.Entry(movie).State = EntityState.Modified;
        }

        public void CreateMovie(Movie movie)
        {
            _dataContext.Movies.Add(movie);
        }

        public void DeleteMovie(Movie movie)
        {
            _dataContext.Entry(movie).State = EntityState.Modified;
        }

        public Task<IPagedResult<MovieOutputDto>> GetListMovies(MovieInputDto movieInput)
        {
            var query = _dataContext.Movies
                .Where(m => m.IsDeleted == false && m.IsApproved == true)
                .AsQueryable();

            return null;
        }

        public Task<Movie> GetListMoviesForEdit(string orderBy, string purpose)
        {
            throw new NotImplementedException();
        }

        public async Task<MovieOutputDto> GetMovieById(int movieId)
        {
            return await _dataContext.Movies
                .Where(m => m.Id == movieId && m.IsDeleted == false)
                .ProjectTo<MovieOutputDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<Movie> GetMovieByIdForEdit(int movieId)
        {
            return await _dataContext.Movies
                .Where(m => m.Id == movieId)
                .SingleOrDefaultAsync();
        }

        public async Task<bool> MovieExits(string movieTitle)
        {
            return await _dataContext.Movies
                .AnyAsync(m => m.Title == movieTitle);
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public void UpdateMovie(Movie movie)
        {
            _dataContext.Entry(movie).State = EntityState.Modified;
        }
    }
}