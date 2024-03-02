using API.DTOs.Movies.Genres;
using API.DTOs.Movies.Movie;
using API.Entities.Movies;
using API.Extentions;
using API.Helpers.Params.Movies;
using API.Interfaces;
using API.Interfaces.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movies
{
    public class MovieController : BaseApiController
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IRatingRepository _ratingRepository;
        private readonly IWatchlistRepository _watchlistRepository;

        public MovieController(IMovieRepository movieRepository, IMapper mapper,
            IPhotoService photoService, IRatingRepository ratingRepository,
            IWatchlistRepository watchlistRepository)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _photoService = photoService;
            _ratingRepository = ratingRepository;
            _watchlistRepository = watchlistRepository;
        }
        #region CreateMovie
        [Authorize]
        [HttpPost("CreateMovie")]
        public async Task<ActionResult> CreateMovie([FromBody] MovieCreateDto movieCreate)
        {
            var userRoles = User.GetRoles();

            if (movieCreate == null) return BadRequest("Invalid data");
            if (await _movieRepository.MovieExits(movieCreate.Title)) return BadRequest("Movie already exists");

            var banner = new Banner
            {
                PublicId = "default_banner",
                Url = "https://res.cloudinary.com/dspm3zys2/image/upload/v1707741602/moviebanner_djgd3a.jpg"
            };
            
            var newMovie = new Movie()
            {
                CreatedId = User.GetUserId(),
                CreatedAt = DateTime.Now,
                CertificationId = movieCreate.CertificationId,
                Banner = banner,
                MovieGenres = movieCreate.GenreIds
                    .Select(genreId => new MovieGenre { GenreId = genreId }).ToList(),
            };

            newMovie.Banner.MovieId = newMovie.Id;

            if (userRoles.Contains("Admin") || userRoles.Contains("Moderator"))
            {
                newMovie.ApprovedId = User.GetUserId();
                newMovie.IsApproved = true;
            }

            _mapper.Map(movieCreate, newMovie);
            _movieRepository.CreateMovie(newMovie);
            return NoContent();
        }
        #endregion

        #region ApproveMovie
        [Authorize]
        [HttpPut("ApproveMovie/{movieId}")]
        public async Task<ActionResult> ApproveMovie(int movieId)
        {
            var movie = await _movieRepository.GetMovieByIdForEdit(movieId);

            if (movie == null) return NotFound();
            if (movie.IsApproved == true) return BadRequest("Movie already approved");

            movie.ApprovedId = User.GetUserId();
            movie.IsApproved = true;

            _movieRepository.ApproveMovie(movie);

            if(await _movieRepository.Save()) return NoContent();
            return BadRequest("Failed to approve movie");
        }
        #endregion

        #region UpdateMovie
        [Authorize]
        [HttpPut("UpdateMovie/{movieId}")]
        public async Task<ActionResult> UpdateMovie(int movieId,[FromBody]MovieUpdateDto movieUpdate)
        {
            var movie = await _movieRepository.GetMovieByIdForEdit(movieId);
            if(movie == null || movie.IsDeleted == true) return NotFound();
            if(movieUpdate == null) return BadRequest("Invalid data");

            movie.UpdatedAt = DateTime.Now;
            movie.UpdatedId = User.GetUserId();
            movie.MovieGenres.Clear();

            foreach (var genreId in movieUpdate.GenreIds)
            {
                movie.MovieGenres.Add(new MovieGenre { MovieId = movie.Id, GenreId = genreId });
            }

            _mapper.Map(movieUpdate, movie);
            _movieRepository.UpdateMovie(movie);
            if (await _movieRepository.Save()) return NoContent();

            return BadRequest("Failed to update movie");
        }
        #endregion

        #region DeleteMovie
        [Authorize]
        [HttpDelete("DeleteMovie/{movieId}")]
        public async Task<ActionResult> DeleteMovie(int movieId)
        {
            var movie = await _movieRepository.GetMovieByIdForEdit(movieId);
            if (movie == null) return NotFound();

            movie.IsDeleted = true;
            movie.DeletedId = User.GetUserId();
            movie.DeletedAt = DateTime.Now;

            _movieRepository.DeleteMovie(movie);
            if(await _movieRepository.Save()) return NoContent();

            return BadRequest("Failed to delete movie");
        }
        #endregion

        #region GetMovieById
        [HttpGet("GetMovieById/{movieId}")]
        public async Task<ActionResult<MovieOutputDto>> GetMovieById(int movieId)
        {
            var movie = await _movieRepository.GetMovieById(movieId);
            if (movie == null) return NotFound();
            var rating = await _ratingRepository.GetListRatings(movieId);
            movie.Genres = await _movieRepository.GetListGenresByMovieId(movieId);
            movie.TotalRatings = rating.Count();
            movie.AverageRating = rating.CalculateRatingScore();
            return Ok(movie);
        }
        #endregion

        #region GetListGenresByMovieId
        [HttpGet("GetListGenresByMovieId/{movieId}")]
        public async Task<ActionResult<GenreOutputDto>> GetListGenresByMovieId(int movieId)
        {
            var genres = await _movieRepository.GetListGenresByMovieId(movieId);
            return Ok(genres);
        }
        #endregion

        #region GetListMovies
        [HttpGet("GetListMovies")]
        public async Task<ActionResult<IEnumerable<ListMoviesOutputDto>>> GetListMovies([FromQuery] MovieParams movieParams)
        {
            int userId = -1;

            if (User.Identity.IsAuthenticated)
            {
                userId = User.GetUserId();
            }

            var movies = await _movieRepository.GetListMovies(movieParams, userId);
            return Ok(movies);
        }
        #endregion
    }
}
