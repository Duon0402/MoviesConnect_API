using API.DTOs.Movies.Ratings;
using API.Entities.Movies;
using API.Extentions;
using API.Interfaces;
using API.Interfaces.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
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

        [HttpPost("AddOrEditRating/{movieId}")]
        public async Task<ActionResult> AddOrEditRating(int movieId, [FromBody] RatingAddOrEditDto ratingAddOrEdit)
        {
            var user = await _userRepository.GetUserById(User.GetUserId());
            var movie = await _movieRepository.GetMovieByIdForEdit(movieId);
            var check = await _ratingRepository.RatingExits(movieId, user.Id);


            if (ratingAddOrEdit == null) return BadRequest("Invalid data");

            if (check == false)
            {
                var newRating = new Rating
                {
                    Movie = movie,
                    AppUser = user
                };

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
        public async Task<ActionResult<IEnumerable<RatingOutputDto>>> GetListRatings(int movieId)
        {
            var ratings = await _ratingRepository.GetListRatings(movieId);
            return Ok(ratings);
        }
    }
}
