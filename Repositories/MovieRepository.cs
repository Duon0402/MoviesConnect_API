using API.Data;
using API.DTOs.Movies;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API.Repositories
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

        public void CreateMovie(MovieDto movie)
        {
            var newMovie = _mapper.Map<Movie>(movie);
            _dataContext.Movies.AddAsync(newMovie);
            _dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Movie>> GetListMoviesAsync()
        {
           return await _dataContext.Movies.ToListAsync();
        }

        public async Task<Movie> GetMovieByIdAsync(int movieId)
        {
            return await _dataContext.Movies
                .Where(m => m.Id == movieId).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
