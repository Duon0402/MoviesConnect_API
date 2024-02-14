using API.DTOs.Movies.Movie;
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
    public class WatchlistController : BaseApiController
    {
        private readonly IWatchlistRepository _watchlistRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IMovieRepository _movieRepository;

        public WatchlistController(IWatchlistRepository watchlistRepository, IMapper mapper,
            IUserRepository userRepository, IMovieRepository movieRepository)
        {
            _watchlistRepository = watchlistRepository;
            _mapper = mapper;
            _userRepository = userRepository;
            _movieRepository = movieRepository;
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


            if(await  _watchlistRepository.ExistWatchlistItem(user.Id, movie.Id))
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
        public async Task<ActionResult<ListMoviesOutputDto>> GetListMoviesFromWatchlist(int userId)
        {
            var movies = await _watchlistRepository.GetListMoviesFromWatchList(userId);

            return Ok(movies);
        }
    }
}
