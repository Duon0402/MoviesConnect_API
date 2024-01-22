using API.Data;
using API.DTOs.Movies;
using API.Entities.Movies;
using API.Helpers.Pagination;
using API.Interfaces.Movies;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PagedList;

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

        public void CreateMovie(MovieDto movie)
        {
            var newMovie = _mapper.Map<Movie>(movie);
            _dataContext.Movies.AddAsync(newMovie);
            _dataContext.SaveChangesAsync();
        }

        public async Task<PagedResults<Movie>> GetPagedListMoviesAsync(MovieInputDto movieInput)
        {
            var query = _dataContext.Movies.AsQueryable();

            var totalItems = await query.CountAsync();
            var pagedMovies = query.ToPagedList(movieInput.PageNumber, movieInput.PageSize);

            var result = new PagedResults<Movie>
            {
                TotalItems = totalItems,
                PagedItems = pagedMovies,
            };

            return result;
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
