using API.Data;
using API.DTOs.Movies.Ratings;
using API.Entities.Movies;
using API.Interfaces.Movies;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Movies
{
    public class RatingRepository : IRatingRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;

        public RatingRepository(DataContext dataContext, IMapper mapper,
            IMovieRepository movieRepository)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _movieRepository = movieRepository;
        }
        public void AddRating(Rating rating)
        {
            _dataContext.Ratings.Add(rating);
        }

        public void EditRating(Rating rating)
        {
            _dataContext.Entry(rating).State = EntityState.Modified;
        }

        public void RemoveRating(Rating rating)
        {
            _dataContext.Ratings.Remove(rating);
        }

        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<Rating> GetRatingForEdit(int movieId, int userId)
        {
            return await _dataContext.Ratings
                .SingleOrDefaultAsync(r => r.MovieId == movieId && r.AppUserId == userId);
        }

        public async Task<IEnumerable<RatingOutputDto>> GetListRatings(int movieId)
        {
            return await _dataContext.Ratings
                .Where(m => m.MovieId == movieId)
                .ProjectTo<RatingOutputDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<bool> RatingExits(int movieId, int userId)
        {
            return await _dataContext.Ratings
                .AnyAsync(r => r.MovieId == movieId && r.AppUserId == userId);
        }

        public async Task<RatingOutputDto> GetRating(int movieId, int userId)
        {
            return await _dataContext.Ratings
                .Where(r => r.MovieId == movieId && r.AppUserId == userId)
                .ProjectTo<RatingOutputDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
    }
}
