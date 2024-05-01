using API.DTOs.Movies.Ratings;
using API.DTOs.Points;
using API.Entities.Movies;
using API.Extentions;
using API.Helpers.Params;
using API.Interfaces;
using API.Interfaces.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class RatingController : BaseApiController
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;
        private readonly IUserRepository _userRepository;

        public RatingController(IRatingRepository ratingRepository, IMapper mapper,
            IMovieRepository movieRepository, IUserRepository userRepository)
        {
            _ratingRepository = ratingRepository;
            _mapper = mapper;
            _movieRepository = movieRepository;
            _userRepository = userRepository;
        }
        [Authorize]
        [HttpPost("AddOrEditRating/{movieId}")]
        public async Task<ActionResult> AddOrEditRating(int movieId, [FromBody] RatingAddOrEditDto ratingAddOrEdit)
        {
            var user = await _userRepository.GetUserById(User.GetUserId());
            var movie = await _movieRepository.GetMovieByIdForEdit(movieId);
            var check = await _ratingRepository.RatingExits(movieId, user.Id);

            var userAge = user.DateOfBirth.CalculateAge();
            if (movie.Certification.MinimumAge > userAge) return BadRequest("Age is not enough to watch this movie.");

            if (ratingAddOrEdit == null) return BadRequest("Invalid data");

            if (check == false)
            {
                var newRating = new Rating
                {
                    Movie = movie,
                    AppUser = user
                };

                string movieTitle = movie.Title;

                // Xác định số điểm cần cộng dựa trên việc có viết review hay không
                int pointsChange = (string.IsNullOrWhiteSpace(ratingAddOrEdit.Review)) ? 10 : 20;

                var pointsTran = new PointTransactionInputDto
                {
                    UserId = user.Id,
                    PointsChange = pointsChange,
                    Description = (string.IsNullOrWhiteSpace(ratingAddOrEdit.Review))
                        ? $"Rating movie: \"{movieTitle}\""
                        : $"Rating and write review for movie: \"{movieTitle}\""
                };
                await _userRepository.UpdateContributionPoints(pointsTran);
                

                _mapper.Map(ratingAddOrEdit, newRating);
                _ratingRepository.AddRating(newRating);
                return Ok();
            }
            else
            {
                var rating = await _ratingRepository.GetRatingForEdit(movie.Id, user.Id);
                
                _mapper.Map(ratingAddOrEdit, rating);
                _ratingRepository.EditRating(rating);
                if (await _ratingRepository.Save()) return Ok();

                return BadRequest("Failed to edit rating");
            }
        }

        [HttpGet("GetRating/{movieId}")]
        public async Task<ActionResult<RatingOutputDto>> GetRating(int movieId)
        {
            var result = await _ratingRepository.GetRating(movieId, User.GetUserId());
            return Ok(result);
        }

        [HttpGet("GetListRatings/{movieId}")]
        public async Task<ActionResult<IEnumerable<RatingOutputDto>>> GetListRatings(int movieId, [FromQuery] RatingParams ratingParams)
        {
            var ratings = await _ratingRepository.GetListRatings(movieId, ratingParams);
            return Ok(ratings);
        }
    }
}
