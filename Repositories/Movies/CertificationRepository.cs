using API.Data;
using API.DTOs.Movies.Certification;
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

        public async Task<IPagedResult<CertificationOutputDto>> GetPagedCertifications(CertificationInputDto certiInput)
        {
            var query = _dataContext.Certifications.AsQueryable();

            if (!string.IsNullOrWhiteSpace(certiInput.Keyword))
            {
                query = query.Where(c => c.Name.Contains(certiInput.Keyword));
            }

            var certi = query
                .ProjectTo<CertificationOutputDto>(_mapper.ConfigurationProvider)
                .ToPagedList(certiInput.PageNumber, certiInput.PageSize);

            return new IPagedResult<CertificationOutputDto>
            {
                PagedItems = certi,
                TotalItems = certi.Count()
            };
        }
    }
}