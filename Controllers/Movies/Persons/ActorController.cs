using API.Data;
using API.DTOs.Movies.Actor;
using API.DTOs.Photos;
using API.Entities.Movies;
using API.Entities.Movies.Persons;
using API.Entities.Users;
using API.Extentions;
using API.Interfaces;
using API.Interfaces.Movies.Persons;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movies.Persons
{
    public class ActorController : BaseApiController
    {
        private readonly IActorReponsitory _actorReponsitory;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public ActorController(IActorReponsitory actorReponsitory, IMapper mapper, IPhotoService photoService)
        {
            _actorReponsitory = actorReponsitory;
            _mapper = mapper;
            _photoService = photoService;
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPost("CreateActor")]
        public async Task<ActionResult> CreateActor([FromBody] ActorInputDto actorInputDto)
        {
            if (actorInputDto == null) return BadRequest("Invalid data");
            if (await _actorReponsitory.ActorExits(actorInputDto.Name)) return BadRequest("Actor already exits");

            var newActor = new Actor();
            newActor.CreatedAt = DateTime.Now;
            newActor.CreatedId = User.GetUserId();

            newActor.ActorImage = new ActorImage()
            {
                PublicId = "default_avatar",
                Url = "https://res.cloudinary.com/dspm3zys2/image/upload/v1707741814/user_yxfmyc.png"
            };

            _mapper.Map(actorInputDto, newActor);
            _actorReponsitory.CreateActor(newActor);
            await _actorReponsitory.Save();

            return Ok(newActor.Id);
        }
        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPut("UpdateActor/{actorId}")]
        public async Task<ActionResult> UpdateActor(int actorId, [FromBody] ActorInputDto actorInputDto)
        {
            if (actorInputDto == null) return BadRequest("Invalid data");

            var actor = await _actorReponsitory.GetActorForEdit(actorId);
            if (actor == null) return NotFound();

            actor.UpdatedAt = DateTime.Now;
            actor.UpdatedId = User.GetUserId();

            _mapper.Map(actorInputDto, actor);
            _actorReponsitory.UpdateActor(actor);

            if(await _actorReponsitory.Save()) return Ok(actor.Id);

            return BadRequest("Failed to update actor");
        }
        [Authorize(Policy = "RequireModeratorRole")]
        [HttpDelete("DeleteActor/{actorId}")]
        public async Task<ActionResult> DeleteActor(int actorId)
        {
            var actor = await _actorReponsitory.GetActorForEdit(actorId);
            if (actor == null) return NotFound();

            actor.DeletedAt = DateTime.Now;
            actor.DeletedId = User.GetUserId();
            actor.IsDeleted = true;

            _actorReponsitory.DeleteActor(actor);
            if (await _actorReponsitory.Save()) return Ok();

            return BadRequest("Failed to delete actor");
        }

        [HttpGet("GetActor/{actorId}")]
        public async Task<ActionResult<ActorOutputDto>> GetActor(int actorId)
        {
            var actor = await _actorReponsitory.GetActor(actorId);
            if (actor == null) return NotFound();
            return Ok(actor);
        }

        [HttpGet("GetListActors")]
        public async Task<ActionResult<IEnumerable<ActorOutputDto>>> GetListActors()
        {
            var actors = await _actorReponsitory.GetListActors();
            return Ok(actors);
        }

        [HttpGet("GetListActorsByMovieId/{movieId}")]
        public async Task<ActionResult<IEnumerable<ActorOutputDto>>> GetListActorsByMovieId(int movieId)
        {
            var actors = await _actorReponsitory.GetListActorsByMovieId(movieId);
            return Ok(actors);
        }

        [HttpGet("GetListMoviesByActorId/{actorId}")]
        public async Task<ActionResult<IEnumerable<ActorOutputDto>>> GetListMoviesByActorId(int actorId)
        {
            var actors = await _actorReponsitory.GetListMoviesByActorId(actorId);
            return Ok(actors);
        }

        [Authorize(Policy = "RequireModeratorRole")]
        [HttpPost("SetActorImage/{actorId}")]
        public async Task<ActionResult<ActorImage>> SetActorImage(int actorId, IFormFile file)
        {
            if (file == null) return BadRequest("File image invalid");
            var actor = await _actorReponsitory.GetActorForEdit(actorId);
            if (actor == null) return NotFound();

            //if (movie.Banner.PublicId != null || movie.Banner.PublicId != "default_banner")
            //{
            //    await _photoService.DeletePhoto(movie.Banner.PublicId);
            //}

            var result = await _photoService.AddPhoto(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var actorImage = new ActorImage
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                ActorId = actorId
            };

            actor.ActorImage = actorImage;

            if (await _actorReponsitory.Save())
            {
                return Ok();
            }

            return BadRequest("Problem adding photo");
        }
    }
}
