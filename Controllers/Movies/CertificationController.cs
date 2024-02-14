using API.DTOs.Movies.Certifications;
using API.Interfaces.Movies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Movies
{
    [Authorize]
    public class CertificationController : BaseApiController
    {
        private readonly ICertificationRepository _certificationRepository;

        public CertificationController(ICertificationRepository certificationRepository)
        {
            _certificationRepository = certificationRepository;
        }

        [HttpGet("GetCertificationById/{certiId}")]
        public async Task<ActionResult> GetCertificationById(int certiId)
        {
            var certi = await _certificationRepository.GetCertificationById(certiId);
            if (certi == null) return NotFound();
            return Ok(certi);
        }

        [HttpGet("GetPagedListCertifications")]
        public async Task<ActionResult> GetPagedListCertifications([FromQuery] CertificationInputDto certiInput)
        {
            var certis = await _certificationRepository.GetPagedListCertifications(certiInput);
            return Ok(certis);
        }
    }
}