using API.DTOs.Movies.Certification;
using API.Helpers.Pagination;

namespace API.Interfaces.Movies
{
    public interface ICertificationRepository
    {
        Task<CertificationOutputDto> GetCertificationById(int certiId);

        Task<IPagedResult<CertificationOutputDto>> GetPagedCertifications(CertificationInputDto certiInput);
    }
}