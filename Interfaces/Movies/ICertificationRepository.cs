using API.DTOs.Movies.Certifications;
using API.Entities.Movies;

namespace API.Interfaces.Movies
{
    public interface ICertificationRepository
    {
        void CreateCertification(CertificationDto certification);
        void UpdateCertification();
        void DeleteCertification();
        Task<Certification> GetCertificationById(int certificationId);
        Task<IEnumerable<Certification>> GetListCertification();
    }
}
