using API.DTOs.Movies.Actor;
using API.DTOs.Movies.Directors;
using API.DTOs.Movies.Movie;
using API.Entities.Movies.Persons;
using API.Extentions;
using API.Interfaces;
using API.Interfaces.Movies.Persons;
using API.Repositories.Movies.Persons;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movies.Persons
{
    public class DirectorController : BaseApiController
    {
        private readonly IDirectorRepository _directorRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public DirectorController(IDirectorRepository directorRepository, IMapper mapper, IPhotoService photoService)
        {
            _directorRepository = directorRepository;
            _mapper = mapper;
            _photoService = photoService;
        }
        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPost("CreateDirector")]
        public async Task<ActionResult> CreateDirector([FromBody] DirectorInputDto directorInput)
        {
            if (directorInput == null) return BadRequest("Invalid data");
            if (await _directorRepository.DirectorExits(directorInput.Name)) return BadRequest("Director already exits");

            var newDirector = new Director();

            newDirector.CreatedAt = DateTime.Now;
            newDirector.CreatedId = User.GetUserId();
            newDirector.DirectorImage = new DirectorImage()
            {
                PublicId = "default_avatar",
                Url = "https://res.cloudinary.com/dspm3zys2/image/upload/v1707741814/user_yxfmyc.png"
            };

            _mapper.Map(directorInput, newDirector);
            _directorRepository.CreateDirector(newDirector);
            await _directorRepository.Save();

            return Ok(newDirector.Id);
        }
        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPut("UpdateDirector/{directorId}")]
        public async Task<ActionResult> UpdateDirector(int directorId, [FromBody] DirectorInputDto directorInput)
        {
            if (directorInput == null) return BadRequest("Invalid data");

            var director = await _directorRepository.GetDirectorForEdit(directorId);
            if (director == null) return NotFound();

            director.UpdatedAt = DateTime.Now;
            director.UpdatedId = User.GetUserId();

            _mapper.Map(directorInput, director);
            _directorRepository.UpdateDirector(director);

            if (await _directorRepository.Save()) return Ok(director.Id);

            return BadRequest("Failed to update director");
        }
        [Authorize(Policy = "RequireModeratorRole")]
        [HttpDelete("DeleteDirector/{directorId}")]
        public async Task<ActionResult> DeleteDirector(int directorId)
        {
            var director = await _directorRepository.GetDirectorForEdit(directorId);
            if (director == null) return NotFound();

            director.DeletedAt = DateTime.Now;
            director.DeletedId = User.GetUserId();
            director.IsDeleted = true;

            _directorRepository.DeleteDirectorr(director);

            if (await _directorRepository.Save()) return Ok(director.Id);

            return BadRequest("Failed to delete director");
        }

        [HttpGet("GetDirector/{directorId}")]
        public async Task<ActionResult<DirectorOutputDto>> GetDirector(int directorId)
        {
            var director = await _directorRepository.GetDirector(directorId);
            if (director == null) return NotFound();
            return Ok(director);
        }

        [HttpGet("GetListDirectors")]
        public async Task<ActionResult<IEnumerable<DirectorOutputDto>>> GetListDirectors()
        {
            var directors = await _directorRepository.GetListDirectors();
            return Ok(directors);
        }

        [HttpGet("GetListMoviesByDirectorId/{directorId}")]
        public async Task<ActionResult<IEnumerable<MovieOutputDto>>> GetListMoviesByDirectorId(int directorId)
        {
            var movies = await _directorRepository.GetListMoviesByDirectorId(directorId);
            return Ok(movies);
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPost("SetDirectorImage/{directorId}")]
        public async Task<ActionResult<DirectorImage>> SetDirectorImage(int directorId, IFormFile file)
        {
            if (file == null) return BadRequest("File image invalid");
            var actor = await _directorRepository.GetDirectorForEdit(directorId);
            if (actor == null) return NotFound();

            //if (movie.Banner.PublicId != null || movie.Banner.PublicId != "default_banner")
            //{
            //    await _photoService.DeletePhoto(movie.Banner.PublicId);
            //}

            var result = await _photoService.AddPhoto(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var directorImage = new DirectorImage
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                DirectorId = directorId
            };

            actor.DirectorImage = directorImage;

            if (await _directorRepository.Save())
            {
                return Ok();
            }

            return BadRequest("Problem adding photo");
        }
    }
}
