using API.DTOs.Movies.Genre;
using API.Entities.Movies;
using API.Extentions;
using API.Interfaces.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movies
{
    public class GenreController : BaseApiController
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenreController(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost("CreateGenre")]
        public async Task<ActionResult> CreateGenre(GenreInputDto inputGenreDto)
        {
            if (inputGenreDto == null) return BadRequest("Invalid Data");

            var newGenre = new Genre()
            {
                Name = inputGenreDto.Name,
                CreatedId = User.GetUserId(),
                CreatedAt = DateTime.Now,
            };

            _genreRepository.CreateGenre(newGenre);
            return Ok();
        }
        [Authorize]
        [HttpPut("UpdateGenre")]
        public async Task<ActionResult> UpdateGenre(int genreId, GenreInputDto inputGenreDto)
        {
            var genre = await _genreRepository.GetGenreById(genreId);
            if (genre == null) return NotFound();

            genre.UpdatedAt = DateTime.Now;
            genre.UpdatedId = User.GetUserId();

            _mapper.Map(inputGenreDto, genre);
            _genreRepository.UpdateGenre(genre);

            if (await _genreRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update genre");
        }
        [Authorize]
        [HttpPut("DeleteGenre")]
        public async Task<ActionResult> DeleteGenre(int genreId)
        {
            var genre = await _genreRepository.GetGenreById(genreId);
            if (genre == null) return NotFound();

            genre.DeletedAt = DateTime.Now;
            genre.DeletedId = User.GetUserId();
            genre.IsDeleted = true;

            _genreRepository.DeleteGenre(genre);

            if (await _genreRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to delete genre");
        }

        [HttpGet("GetGenreById")]
        public async Task<ActionResult<Genre>> GetGenreById(int genreId)
        {
           var genre = await _genreRepository.GetGenreById(genreId);
           if(genre == null) return NotFound();
           return Ok(genre);
        }

        [HttpGet("GetListGenres")]
        public async Task<ActionResult<Genre>> GetListGenres()
        {
            var genres = await _genreRepository.GetListGenres();
            if (genres == null) return NotFound();
            return Ok(genres);
        }
    }
}
