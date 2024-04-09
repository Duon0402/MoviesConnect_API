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

        [HttpGet("GetListCertifications")]
        public async Task<ActionResult<IEnumerable<CertificationOutputDto>>> GetListCertifications()
        {
            var certis = await _certificationRepository.GetListCertifications();
            return Ok(certis);
        }
    }
}