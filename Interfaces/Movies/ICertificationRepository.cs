using API.DTOs.Movies.Certifications;
using API.Entities.Movies;
using API.Helpers.Pagination;

namespace API.Interfaces.Movies
{
    public interface ICertificationRepository
    {
        void CreateCertifi(Certification certification);
        void UpdateCertifi(Certification certification);
        void DeleteCertifi(Certification certification);
        Task<bool> CertifiExits(string certifiName);
        Task<bool> Save();
        Task<CertificationOutputDto> GetCertificationById(int certiId);
        Task<Certification> GetCertificationByIdForEdit(int certiId);
        Task<IEnumerable<CertificationOutputDto>> GetListCertifications();
    }
}