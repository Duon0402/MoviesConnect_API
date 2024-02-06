using API.DTOs.Movies.Certification;
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

        [HttpGet("GetCertificationById")]
        public async Task<ActionResult> GetCertificationById([FromQuery] int certiId)
        {
            var certi = await _certificationRepository.GetCertificationById(certiId);
            if (certi == null) return NotFound();
            return Ok(certi);
        }

        [HttpGet("GetPagedCertifications")]
        public async Task<ActionResult> GetPagedCertifications([FromBody] CertificationInputDto certiInput)
        {
            var certis = await _certificationRepository.GetPagedCertifications(certiInput);
            return Ok(certis);
        }
    }
}