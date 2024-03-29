﻿using API.DTOs.Movies.Genres;
using API.Entities.Movies;
using API.Extentions;
using API.Interfaces.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movies
{
    [Authorize]
    public class GenreController : BaseApiController
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenreController(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        [HttpPost("CreateGenre")]
        public async Task<ActionResult> CreateGenre([FromBody] GenreCreateDto genreCreate)
        {
            if (genreCreate == null) return BadRequest();

            if (await _genreRepository.GenreExits(genreCreate.Name)) return BadRequest("Genre already exists");

            var newGenre = new Genre
            {
                CreatedId = User.GetUserId(),
                CreatedAt = DateTime.Now
            };

            _mapper.Map(genreCreate, newGenre);
            _genreRepository.CreateGenre(newGenre);

            return NoContent();
        }

        [HttpPut("UpdateGenre/{genreId}")]
        public async Task<ActionResult> UpdateGenre(int genreId, [FromBody] GenreUpdateDto updateDto)
        {
            var genre = await _genreRepository.GetGenreByIdForEdit(genreId);
            if (genre == null || genre.IsDeleted == true) return NotFound();
            genre.UpdatedId = User.GetUserId();
            genre.UpdatedAt = DateTime.Now;

            _mapper.Map(updateDto, genre);
            _genreRepository.UpdateGenre(genre);

            if (await _genreRepository.Save()) return NoContent();

            return BadRequest("Failed to update genre");
        }

        [HttpDelete("DeleteGenre/{genreId}")]
        public async Task<ActionResult> DeleteGenre(int genreId)
        {
            var genre = await _genreRepository.GetGenreByIdForEdit(genreId);
            if (genre == null || genre.IsDeleted == true) return NotFound();
            genre.DeletedId = User.GetUserId();
            genre.DeletedAt = DateTime.Now;
            genre.IsDeleted = true;

            _genreRepository.DeleteGenre(genre);

            if (await _genreRepository.Save()) return NoContent();

            return BadRequest("Failed to delete genre");
        }

        [HttpGet("GetGenreById/{genreId}")]
        public async Task<ActionResult> GetGenreById(int genreId)
        {
            var genre = await _genreRepository.GetGenreById(genreId);

            if (genre == null) return NotFound();

            return Ok(genre);
        }

        [HttpGet("GetPagedListGenres")]
        public async Task<ActionResult> GetPagedListGenres([FromQuery] GenreInputDto genreInput)
        {
            var genres = await _genreRepository.GetPagedListGenres(genreInput);
            return Ok(genres);
        }

        [HttpGet("GetListGenres")]
        public async Task<ActionResult<IEnumerable<GenreOutputDto>>> GetListGenres([FromQuery] string? keyword)
        {
            var genres = await _genreRepository.GetListGenres(keyword);
            return Ok(genres);
        }

        [HttpGet("GetListMoviesByGenreId/{genreId}")]
        public async Task<ActionResult> GetListMoviesByGenreId(int genreId)
        {
            var genres = await _genreRepository.GetListMoviesByGenreId(genreId);
            return Ok(genres);
        }
    }
}