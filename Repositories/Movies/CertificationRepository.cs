using API.Data;
using API.DTOs.Movies.Certifications;
using API.Entities.Movies;
using API.Helpers.Pagination;
using API.Interfaces.Movies;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using PagedList;

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

        public async Task<bool> CertifiExits(string certifiName)
        {
            return await _dataContext
                .Certifications
                .AnyAsync(c => c.Name == certifiName);
        }

        public void CreateCertifi(Certification certification)
        {
            _dataContext.Add(certification);
        }

        public void DeleteCertifi(Certification certification)
        {
            _dataContext.Entry(certification).State = EntityState.Modified;
        }

        public async Task<CertificationOutputDto> GetCertificationById(int certiId)
        {
            return await _dataContext.Certifications
                .Where(c => c.Id == certiId && c.IsDeleted == false)
                .ProjectTo<CertificationOutputDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }

        public async Task<Certification> GetCertificationByIdForEdit(int certiId)
        {
            return await _dataContext.Certifications
                .SingleOrDefaultAsync(c => c.Id == certiId);
        }

        public async Task<IEnumerable<CertificationOutputDto>> GetListCertifications()
        {
            return await _dataContext.Certifications
                .Where(c => c.IsDeleted == false)
                .OrderBy(c => c.Id)
                .ProjectTo<CertificationOutputDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
        public async Task<bool> Save()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public void UpdateCertifi(Certification certification)
        {
            _dataContext.Entry(certification).State = EntityState.Modified;
        }
    }
}