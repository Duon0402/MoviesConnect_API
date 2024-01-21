using API.DTOs.Movies;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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
        public async Task<ActionResult> CreateMovie(MovieDto movie)
        {
            if (movie == null) return BadRequest("Data invalid");
            _movieRepository.CreateMovie(movie);
            return Ok(movie);
        }
        [HttpGet("GetMovieById/{movieId}")]
        public async Task<ActionResult<Movie>> GetMovieById(int movieId)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(movieId);
            if (movie == null) return NotFound();
            return Ok(movie);
        }
    }
}
