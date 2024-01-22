using API.DTOs.Movies;
using API.DTOs.Movies.Certifications;
using API.Entities.Movies;
using API.Interfaces.Movies;
using API.Repositories.Movies;
using Microsoft.AspNetCore.Mvc;
using PagedList;

namespace API.Controllers.Movies
{
    public class CertificationController : BaseApiController
    {
        private readonly ICertificationRepository _certificationRepository;

        public CertificationController(ICertificationRepository certificationRepository)
        {
            _certificationRepository = certificationRepository;
        }

        [HttpPost("CreateCertification")]
        public async Task<ActionResult> CreateCertification(CertificationDto certification)
        {
            if(certification == null) { return BadRequest("Data invalid"); }

            _certificationRepository.CreateCertification(certification);
            return Ok(certification);
        }

        [HttpGet("GetListCertification")]
        public async Task<ActionResult<IEnumerable<Certification>>> GetListCertification()
        {
            var certifications = await _certificationRepository.GetListCertification();
            return Ok(certifications);
        }

        [HttpGet("GetCertificationById")]
        public async Task<ActionResult<Certification>> GetCertificationById(int certificationId)
        {
            var certification = await _certificationRepository.GetCertificationById(certificationId);
            return Ok(certification);
        }
    }
}
