using API.DTOs.Movies.Certifications;
using API.Entities.Movies;
using API.Extentions;
using API.Interfaces.Movies;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movies
{
    public class CertificationController : BaseApiController
    {
        private readonly ICertificationRepository _certificationRepository;
        private readonly IMapper _mapper;

        public CertificationController(ICertificationRepository certificationRepository,
            IMapper mapper)
        {
            _certificationRepository = certificationRepository;
            _mapper = mapper;
        }

        [HttpGet("GetCertificationById/{certiId}")]
        public async Task<ActionResult<CertificationOutputDto>> GetCertificationById(int certiId)
        {
            var certi = await _certificationRepository.GetCertificationById(certiId);
            if (certi == null) return NotFound();
            return Ok(certi);
        }

        [HttpGet("GetListCertifications")]
        public async Task<ActionResult<IEnumerable<CertificationOutputDto>>> GetListCertifications()
        {
            var certis = await _certificationRepository.GetListCertifications();
            return Ok(certis);
        }
        [Authorize(Policy = "RequireModeratorRole")]
        #region CreateOrEditCertifi
        [HttpPost("CreateOrEditCertifi/{certiId}")]
        public async Task<ActionResult> CreateCertifi(int certiId,
            [FromBody] CertificationCreateOrEditDto certificationCreateOrEdit)
        {
            if (certificationCreateOrEdit == null) return BadRequest("Invalid data");
            // create
            if (certiId == 0)
            {
                var newCerti = new Certification();
                newCerti.CreatedAt = DateTime.Now;
                newCerti.CreatedId = User.GetUserId();

                _mapper.Map(certificationCreateOrEdit, newCerti);
                _certificationRepository.CreateCertifi(newCerti);
                return NoContent();
            }
            //edit
            else
            {
                var certi = await _certificationRepository.GetCertificationByIdForEdit(certiId);
                if(certi == null) return NotFound();

                certi.UpdatedAt = DateTime.Now;
                certi.UpdatedId = User.GetUserId();

                _mapper.Map(certificationCreateOrEdit, certi);
                _certificationRepository.UpdateCertifi(certi);
                if (await _certificationRepository.Save()) return Ok();
                return BadRequest("Update failure");
            }
        }
        #endregion
        [Authorize(Policy = "RequireModeratorRole")]
        [HttpDelete("DeleteCertification/{certiId}")]
        public async Task<ActionResult> DeleteCertification(int certiId)
        {
            var certi = await _certificationRepository.GetCertificationByIdForEdit(certiId);
            if (certi == null) return NotFound();

            certi.DeletedAt = DateTime.Now;
            certi.DeletedId = User.GetUserId();
            certi.IsDeleted = true;

            _certificationRepository.DeleteCertifi(certi);
            if (await _certificationRepository.Save()) return Ok();
            return BadRequest("Delete failure");
        }
    }
}