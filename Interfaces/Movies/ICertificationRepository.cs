using API.DTOs.Movies.Certifications;
using API.Entities.Movies;
using API.Helpers.Pagination;

namespace API.Interfaces.Movies
{
    public interface ICertificationRepository
    {
        Task<CertificationOutputDto> GetCertificationById(int certiId);
        Task<Certification> GetCertificationByIdForEdit(int certiId);
        Task<IEnumerable<CertificationOutputDto>> GetListCertifications();
    }
}