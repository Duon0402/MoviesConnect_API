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

        public async Task<CertificationOutputDto> GetCertificationById(int certiId)
        {
            return await _dataContext.Certifications
                .ProjectTo<CertificationOutputDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(c => c.Id == certiId);
        }

        public async Task<Certification> GetCertificationByIdForEdit(int certiId)
        {
            return await _dataContext.Certifications
                .SingleOrDefaultAsync(c => c.Id == certiId);
        }

        public async Task<IEnumerable<CertificationOutputDto>> GetListCertifications()
        {
            return await _dataContext.Certifications
                .OrderBy(c => c.Id)
                .ProjectTo<CertificationOutputDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }
    }
}