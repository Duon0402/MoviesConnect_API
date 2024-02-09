using API.DTOs.Movies.Genres;
using API.DTOs.Movies.Movie;
using API.Entities.Movies;
using API.Extentions;
using API.Helpers.Params.Movies;
using API.Interfaces.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movies
{
    [Authorize]
    public class MovieController : BaseApiController
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieController(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }

        [HttpPost("CreateMovie")]
        public async Task<ActionResult> CreateMovie([FromBody] MovieCreateDto movieCreate)
        {
            var userRoles = User.GetRoles();

            if (movieCreate == null) return BadRequest("Invalid data");
            if (await _movieRepository.MovieExits(movieCreate.Title)) return BadRequest("Movie already exists");

            var newMovie = new Movie()
            {
                CreatedId = User.GetUserId(),
                CreatedAt = DateTime.Now,
                CertificationId = movieCreate.CertificationId,
                MovieGenres = movieCreate.GenreIds
                    .Select(genreId => new MovieGenre { GenreId = genreId }).ToList()
            };

            if (userRoles.Contains("Admin") || userRoles.Contains("Moderator"))
            {
                newMovie.ApprovedId = User.GetUserId();
                newMovie.IsApproved = true;
            }

            _mapper.Map(movieCreate, newMovie);
            _movieRepository.CreateMovie(newMovie);
            return NoContent();
        }

        [HttpPut("ApproveMovie")]
        public async Task<ActionResult> ApproveMovie([FromQuery] int movieId)
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

        [HttpPut("UpdateMovie")]
        public async Task<ActionResult> UpdateMovie([FromQuery] int movieId,[FromBody]MovieUpdateDto movieUpdate)
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

        [HttpDelete("DeleteMovie")]
        public async Task<ActionResult> DeleteMovie([FromQuery] int movieId)
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

        [HttpGet("GetMovieById")]
        public async Task<ActionResult<MovieOutputDto>> GetMovieById([FromQuery] int movieId)
        {
            var movie = await _movieRepository.GetMovieById(movieId);
            if (movie == null) return NotFound();

            movie.Genres = await _movieRepository.GetListGenresByMovieId(movieId);

            return Ok(movie);
        }

        [HttpGet("GetListGenresByMovieId")]
        public async Task<ActionResult<GenreOutputDto>> GetListGenresByMovieId([FromQuery] int movieId)
        {
            var genres = await _movieRepository.GetListGenresByMovieId(movieId);
            return Ok(genres);
        }

        [HttpGet("GetListMovies")]
        public async Task<ActionResult<IEnumerable<MovieOutputDto>>> GetListMovies([FromQuery] MovieParams movieParams)
        {
            var movies = await _movieRepository.GetListMovies(movieParams);
            return Ok(movies);
        }
    }
}
