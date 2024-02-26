using API.DTOs.Movies.Movie;
using API.Entities.Movies;
using API.Extentions;
using API.Interfaces;
using API.Interfaces.Movies;
using API.Repositories.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace API.Controllers
{
    [Authorize]
    public class WatchlistController : BaseApiController
    {
        private readonly IWatchlistRepository _watchlistRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IRatingRepository _ratingRepository;

        public WatchlistController(IWatchlistRepository watchlistRepository, IMapper mapper,
            IUserRepository userRepository, IMovieRepository movieRepository, IRatingRepository ratingRepository)
        {
            _watchlistRepository = watchlistRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _movieRepository = movieRepository;
            _ratingRepository = ratingRepository;
        }

        [HttpPost("AddMovieToWatchList/{movieId}")]
        public async Task<ActionResult> RemoveMovieFromWatchlist(int movieId)
        {
            var user = await _userRepository.GetUserById(User.GetUserId());
            var movie = await _movieRepository.GetMovieByIdForEdit(movieId);

            if (movie == null || user == null) return BadRequest();
            if (movie.IsDeleted == true || movie.IsApproved == false)
            {
                return NotFound("Movie not found");
            }
            var userAge = user.DateOfBirth.CalculateAge();
            if (movie.Certification.MinimumAge > userAge) return BadRequest("Age is not enough to watch this movie.");


            if (await  _watchlistRepository.ExistWatchlistItem(user.Id, movie.Id))
            {
                return BadRequest("Movie already exists in the watchlist.");
            }

            var watchList = new Watchlist
            {
                AppUser = user,
                Movie = movie
            };

            _watchlistRepository.AddMovieToWatchList(watchList);
            return Ok();
        }

        [HttpDelete("RemoveMovieFromWatchlist/{movieId}")]
        public async Task<ActionResult> AddMovieToWatchList(int movieId)
        {
            var user = await _userRepository.GetUserById(User.GetUserId());
            var movie = await _movieRepository.GetMovieByIdForEdit(movieId);

            if (movie == null || user == null) return BadRequest();

            var watchList = new Watchlist
            {
                AppUser = user,
                Movie = movie
            };

            _watchlistRepository.RemoveMovieFromWatchList(watchList);
            return Ok();
        }

        [HttpGet("GetListMoviesFromWatchlist/{userId}")]
        public async Task<ActionResult<IEnumerable<ListMoviesOutputDto>>> GetListMoviesFromWatchlist(int userId)
        {
            var movies = await _watchlistRepository.GetListMoviesFromWatchList(userId);
            foreach (var movie in movies)
            {
                movie.IsInWatchList = await _watchlistRepository.ExistWatchlistItem(User.GetUserId() ,movie.Id);
                var ratings = await _ratingRepository.GetListRatings(movie.Id);
                movie.TotalRatings = ratings.Count();
                movie.AverageRating = ratings.CalculateRatingScore();
                movie.IsInWatchList = await _watchlistRepository.ExistWatchlistItem(User.GetUserId(), movie.Id);
            }
            return Ok(movies);
        }
    }
}
