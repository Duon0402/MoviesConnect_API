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

        [HttpPost("AddRating")]
        public async Task<ActionResult> AddRating([FromQuery] int movieId, [FromBody] RatingCreateDto ratingCreate)
        {
            if (ratingCreate == null) return BadRequest("Invalid data");

            var user = await _userRepository.GetUserByUsername(User.GetUsername());
            var movie = await _movieRepository.GetMovieByIdForEdit(movieId);

            if (movie == null || user == null) return BadRequest();
            if (movie.IsDeleted == true || movie.IsApproved == false) return NotFound("Movie not found");

            var newRating = new Rating
            {
                Movie = movie,
                AppUser = user
            };

            _mapper.Map(ratingCreate, newRating);
            _ratingRepository.AddRating(newRating);
            return Ok();
        }

        [HttpPut("EditRating")]
        public async Task<ActionResult> EditRating([FromQuery] int ratingId, int movieId, int userId,
            [FromBody] RatingUpdateDto ratingUpdate)
        {
            var rating = await _ratingRepository.GetRatingForEdit(ratingId);

            if (rating == null) return NotFound();

            if (rating.MovieId != movieId || rating.AppUserId != userId) return BadRequest("Failed to edit rating");

            _mapper.Map(ratingUpdate, rating);
            _ratingRepository.EditRating(rating);
            if(await _ratingRepository.Save()) return Ok();

            return BadRequest("Failed to edit rating");
        }

        [HttpGet("GetListRatings")]
        public async Task<ActionResult<IEnumerable<RatingOutputDto>>> GetListRatings([FromQuery] int movieId)
        {
            var result = await _ratingRepository.GetListRatings(movieId);
            return Ok(result);
        }
    }
}
