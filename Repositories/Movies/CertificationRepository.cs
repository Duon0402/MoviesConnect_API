using API.Data;
using API.DTOs.Movies.Certifications;
using API.Entities.Movies;
using API.Interfaces.Movies;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Movies
{
    public class CertificationRepository : ICertificationRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public CertificationRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public void CreateCertification(CertificationDto certification)
        {
            var newCertification = _mapper.Map<Certification>(certification);
            _dataContext.Certifications.Add(newCertification);
            _dataContext.SaveChangesAsync();
        }

        public void DeleteCertification()
        {
            throw new NotImplementedException();
        }

        public async Task<Certification> GetCertificationById(int certificationId)
        {
            return await _dataContext.Certifications
                .Where(c => c.Id == certificationId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Certification>> GetListCertification()
        {
            return await _dataContext.Certifications.ToListAsync();
        }

        public void UpdateCertification()
        {
            throw new NotImplementedException();
        }
    }
}
