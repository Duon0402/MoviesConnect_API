using API.Data;
using API.DTOs.Movies.Ratings;
using API.Entities.Movies;
using API.Helpers.Params;
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

        public async Task<IEnumerable<RatingOutputDto>> GetListRatings(int movieId, RatingParams ratingParams)
        {
            var query = _dataContext.Ratings
                .Where(r => r.MovieId == movieId)
                .Where(r => r.IsDeleted == false)
                .AsQueryable();

            if (ratingParams.Score.HasValue)
            {
                query = query.Where(r => r.Score == ratingParams.Score);
            }

            if (ratingParams.RatingViolation.HasValue && ratingParams.RatingViolation == true)
            {
                query = query.Where(r => !r.RatingViolation);
            }

            var result = await query.Select(r => new RatingOutputDto
            {
                Score = r.Score,
                Review = r.Review,
                AppUserId = r.AppUserId,
                Username = _dataContext.Users.FirstOrDefault(u => u.Id == r.AppUserId).UserName
            }).ToListAsync();

            return result;
        }

        public async Task<bool> RatingExits(int movieId, int userId)
        {
            return await _dataContext.Ratings
                .AnyAsync(r => r.MovieId == movieId && r.AppUserId == userId);
        }

        public async Task<RatingOutputDto> GetRating(int movieId, int userId)
        {

            var ratingDto = await _dataContext.Ratings
                .Include(r => r.AppUser)
                .FirstOrDefaultAsync(r => r.MovieId == movieId && r.AppUserId == userId);
            
            if (ratingDto == null)
            {
                return null;
            }

            var ratingOutputDto = new RatingOutputDto
            {
                Score = ratingDto.Score,
                Review = ratingDto.Review,
                AppUserId = userId,
                Username = ratingDto.AppUser.UserName
            };

            return ratingOutputDto;
        }
    }
}
